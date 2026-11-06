using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniSupermarket.API;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Thiếu ConnectionStrings:DefaultConnection trong appsettings.json");
var jwtSecret = builder.Configuration["JwtSettings:Secret"]
    ?? throw new InvalidOperationException("Thiếu JwtSettings:Secret trong appsettings.json");

// DbContext dung SQL Server
builder.Services.AddDbContext<SupermarketDbContext>(options =>
    options.UseSqlServer(connectionString));

// Dich vu ADO.NET
builder.Services.AddScoped<AdoNetService>();

// CORS: cho phep client (WinForms, trang web test...) goi API tu nguon khac
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWinForms", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});


builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents {
        OnMessageReceived = context => {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader)) {
                var token = authHeader.Trim();
                // Cat bo tat ca tien to "Bearer " lap lai (VD: "Bearer Bearer eyJ...")
                while (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
                    token = token.Substring("Bearer ".Length).Trim();
                }
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});

// dich vu phan quyen
builder.Services.AddAuthorization();

// duong dan API chu thuong: api/categories, api/products
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// dang ky Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiniSupermarket API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Description = "Nhập JWT Token của bạn (Có thể dán chuỗi token trực tiếp hoặc dán kèm chữ Bearer)",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// khoi tao csdl
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<SupermarketDbContext>();

        // CSDL da co san bang (tao tu truoc, chua co lich su migration)
        // -> danh dau cac migration la da ap dung de EF khong tao lai bang
        bool hasUsersTable = context.Database
            .SqlQueryRaw<int>("SELECT COUNT(*) AS [Value] FROM sys.tables WHERE name = 'Users'")
            .AsEnumerable().First() > 0;
        if (hasUsersTable && !context.Database.GetAppliedMigrations().Any())
        {
            context.Database.ExecuteSqlRaw(@"
                IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
                    CREATE TABLE __EFMigrationsHistory (
                        MigrationId nvarchar(150) NOT NULL PRIMARY KEY,
                        ProductVersion nvarchar(32) NOT NULL)");
            foreach (string migrationId in context.Database.GetMigrations())
            {
                context.Database.ExecuteSqlRaw(
                    "INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ({0}, {1})",
                    migrationId, "8.0.13");
            }
        }

        // tao bang & nap Seed Data (CSDL moi)
        context.Database.Migrate();

        // luon co 3 tai khoan mac dinh (mat khau 123456), chi tao neu chua co
        var defaultAccounts = new[]
        {
            new { Username = "admin", FullName = "Quản Trị Viên Mặc Định", Role = AppRoles.Admin },
            new { Username = "cashier", FullName = "Thu Ngân Mặc Định", Role = AppRoles.Cashier },
            new { Username = "warehouse", FullName = "Thủ Kho Mặc Định", Role = AppRoles.Warehouse }
        };
        foreach (var acc in defaultAccounts)
        {
            if (!await context.Users.AnyAsync(u => u.Username == acc.Username))
            {
                context.Users.Add(new MiniSupermarket.API.Models.User
                {
                    Username = acc.Username,
                    Password = "123456",
                    FullName = acc.FullName,
                    Role = acc.Role,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                });
            }
        }
        await context.SaveChangesAsync();

        // tao/cap nhat Stored Procedure sp_BaoCaoDoanhThuCa
        var ado = services.GetRequiredService<AdoNetService>();
        await ado.EnsureStoredProceduresAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Lỗi khi khởi tạo CSDL (Migrate / Stored Procedure)");
    }
}

// pipeline xu ly request/response

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWinForms");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
