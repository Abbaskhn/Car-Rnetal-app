using application.Dbcontext;
using application.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly Dbuser _customer;
        public CustomerController(Dbuser cstmr)
        {
            _customer = cstmr;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var a = await _customer.customers.ToListAsync();
            return Ok(a);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var a = await _customer.customers.FirstOrDefaultAsync(x => x.Id == id);
            if (a == null)
            {
                return NotFound("NotFound");
            }
            return Ok(a);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Customers data)
        {
            _customer.customers.Add(data);
            _customer.SaveChanges();
            return Ok(data);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Customers data)
        {
            var a = await _customer.customers.FirstOrDefaultAsync(x => x.Id == id);
            if (a == null)
            {
                return NotFound("NotFound");
            }
            a.Name = data.Name;
            a.Email = data.Email;
            a.Phone = data.Phone;
            a.Email = data.Email;

            await _customer.SaveChangesAsync();
            return Ok(a);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _customer.customers.FirstOrDefaultAsync(x => x.Id == id);
            if (a == null)
            {
                return NotFound("NotFound");
            }
            _customer.customers.Remove(a);
            await _customer.SaveChangesAsync();
            return Ok(a);
        }
    }
}
