using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniSupermarket.API.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ZenMart_HoMaiPhucThinh;Trusted_Connection=True;TrustServerCertificate=True;";
}

builder.Services.AddDbContext<SupermarketDbContext>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình dịch vụ xác thực JWT Bearer
var jwtSecret = builder.Configuration["JwtSettings:Secret"] ?? "SupermarketSecretKeyDoAnMonHoc2026SecureString!!";
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    // Tự động nhận diện token thông minh: Hỗ trợ cả trường hợp dán thiếu chữ "Bearer " hoặc dán thừa
    options.Events = new JwtBearerEvents {
        OnMessageReceived = context => {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader)) {
                var token = authHeader.Trim();
                while (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
                    token = token.Substring("Bearer ".Length).Trim();
                }
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Cấu hình Swagger UI hỗ trợ nút Authorize cho Bearer Token
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

// Tự động khởi tạo Database và nạp 15 dữ liệu mẫu (Seed Data) nếu chưa có
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SupermarketDbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi khi tự động khởi tạo CSDL SupermarketDbContext");
    }
}

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Tắt chuyển hướng HTTPS bắt buộc để WinForms Client gọi qua http://localhost:5000 không bị 307 Redirect mất Token
// app.UseHttpsRedirection();

// Bắt buộc gọi UseAuthentication trước UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
