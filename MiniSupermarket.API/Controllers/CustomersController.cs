using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Dtos;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AppRoles.AdminOrCashier)]
    public class CustomersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public CustomersController(SupermarketDbContext context)
        {
            _context = context;
        }

        // Ham phu: Entity -> DTO
        private static CustomerDto ToDto(Customer c) => new CustomerDto
        {
            CustomerId = c.CustomerId,
            CustomerName = c.CustomerName,
            PhoneNumber = c.PhoneNumber,
            Address = c.Address,
            RewardPoints = c.RewardPoints,
            MembershipRank = c.MembershipRank
        };

        // GET/api/customers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Customers
                .AsNoTracking()
                .OrderBy(c => c.CustomerId)
                .Select(c => new CustomerDto
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    RewardPoints = c.RewardPoints,
                    MembershipRank = c.MembershipRank
                })
                .ToListAsync();
            return Ok(list);
        }

        // GET/api/customers/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.CustomerId == id);
            if (customer == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng!" });   // 404
            }
            return Ok(ToDto(customer));
        }

        // GET/api/customers/by-phone/{phone} -> POS tra cuu khach theo SDT
        [HttpGet("by-phone/{phone}")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return BadRequest(new { message = "Số điện thoại không hợp lệ!" });   // 400
            }

            string p = phone.Trim();
            var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.PhoneNumber == p);
            if (customer == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng!" });     // 404
            }
            return Ok(ToDto(customer));
        }

        // GET/api/customers/search?keyword=...
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Vui lòng nhập từ khóa tìm kiếm!" });
            }

            string kw = keyword.Trim();
            var result = await _context.Customers
                .AsNoTracking()
                .Where(c => c.CustomerName.Contains(kw) || c.PhoneNumber.Contains(kw))
                .Select(c => new CustomerDto
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    RewardPoints = c.RewardPoints,
                    MembershipRank = c.MembershipRank
                })
                .ToListAsync();

            return Ok(result);
        }

        // POST/api/customers
        [HttpPost]
        [Authorize(Roles = AppRoles.AdminGroup)]
        public async Task<IActionResult> Create([FromBody] CustomerCreateUpdateDto dto)
        {
            string phone = dto.PhoneNumber.Trim();
            if (await _context.Customers.AnyAsync(c => c.PhoneNumber == phone))
            {
                return BadRequest(new { message = "Số điện thoại này đã được đăng ký!" });   // 400
            }

            var customer = new Customer
            {
                CustomerName = dto.CustomerName.Trim(),
                PhoneNumber = phone,
                Address = dto.Address?.Trim(),
                RewardPoints = dto.RewardPoints,
                MembershipRank = string.IsNullOrWhiteSpace(dto.MembershipRank) ? "Chuẩn" : dto.MembershipRank.Trim()
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId }, ToDto(customer));   // 201
        }

        // PUT/api/customers/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = AppRoles.AdminGroup)]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerCreateUpdateDto dto)
        {
            var existing = await _context.Customers.FindAsync(id);
            if (existing == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng cần cập nhật!" });
            }

            string phone = dto.PhoneNumber.Trim();
            if (await _context.Customers.AnyAsync(c => c.PhoneNumber == phone && c.CustomerId != id))
            {
                return BadRequest(new { message = "Số điện thoại này đã thuộc về khách hàng khác!" });
            }

            existing.CustomerName = dto.CustomerName.Trim();
            existing.PhoneNumber = phone;
            existing.Address = dto.Address?.Trim();
            existing.RewardPoints = dto.RewardPoints;
            if (!string.IsNullOrWhiteSpace(dto.MembershipRank))
            {
                existing.MembershipRank = dto.MembershipRank.Trim();
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //bDELETE/api/customers/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = AppRoles.AdminGroup)]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng cần xóa!" });
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
