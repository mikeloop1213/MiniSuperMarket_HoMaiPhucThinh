using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public CustomersController(SupermarketDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Customers.AsNoTracking().ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng!" });
            }

            return Ok(customer);
        }

        [HttpGet("by-phone/{phone}")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return BadRequest(new { message = "Số điện thoại không hợp lệ!" });
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == phone.Trim());
            if (customer == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng!" });
            }

            return Ok(customer);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Vui lòng nhập từ khóa tìm kiếm!" });
            }

            var result = await _context.Customers
                .Where(c => c.CustomerName.Contains(keyword) || c.PhoneNumber.Contains(keyword))
                .ToListAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId }, customer);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Customer customer)
        {
            var existing = await _context.Customers.FindAsync(id);
            if (existing == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng cần cập nhật!" });
            }

            existing.CustomerName = customer.CustomerName;
            existing.PhoneNumber = customer.PhoneNumber;
            existing.Address = customer.Address;
            existing.RewardPoints = customer.RewardPoints;
            existing.MembershipRank = customer.MembershipRank;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
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
