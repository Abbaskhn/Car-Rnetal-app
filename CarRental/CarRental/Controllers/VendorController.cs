using application.Dbcontext;
using application.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly Dbuser _vendor;
        public VendorController(Dbuser vendor)
        {
            _vendor = vendor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var a = await _vendor.Vendors.ToListAsync();
            return Ok(a);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Vendor data)
        {
            try
            {
                _vendor.Vendors.Add(data);
                _vendor.SaveChanges();
                return Ok(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Vendor data)
        {
            var a = await _vendor.Vendors.FirstOrDefaultAsync(x => x.Id == id);
            if (a == null)
            {
                return NotFound("NotFound");
            }
            a.Name = data.Name;
            a.Email = data.Email;
            a.Phone = data.Phone;
            a.Company = data.Company;


            await _vendor.SaveChangesAsync();
            return Ok(a);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _vendor.Vendors.FirstOrDefaultAsync(x => x.Id == id);
            if (a == null)
            {
                return NotFound("NotFound");
            }
            _vendor.Vendors.Remove(a);
            await _vendor.SaveChangesAsync();
            return Ok(a);
        }

    }
}

