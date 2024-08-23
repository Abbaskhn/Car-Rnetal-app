using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace CRCQRS.API.DataSeeding
{
  public interface IDataSeeder
  {
    Task SeedAsync();
  }

  // Infrastructure Layer
  public class DataSeeder : IDataSeeder
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly CRCQRSContext _dbContext;
    public DataSeeder(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, CRCQRSContext dbContext)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _dbContext = dbContext;
    }

    public async Task SeedAsync()
    {
      await SeedRoles();
      await SeedUsers();
    }

    private async Task SeedRoles()
    {
      if (!await _roleManager.RoleExistsAsync("Admin"))
      {
        var role = new ApplicationRole { Name = "Admin", Description = "Administrator of the system" };
        await _roleManager.CreateAsync(role);
      }
      if (!await _roleManager.RoleExistsAsync("Customer"))
      {
        var role = new ApplicationRole { Name = "Customer", Description = "A Person who can Book a car" };
        await _roleManager.CreateAsync(role);
      }
      if (!await _roleManager.RoleExistsAsync("Vendor"))
      {
        var role = new ApplicationRole { Name = "Vendor", Description = "A Person who can provide his cars for renting" };
        await _roleManager.CreateAsync(role);
      }
    }


    private async Task SeedUsers()
    {
      if (await _userManager.FindByEmailAsync("waqar@habib.com") == null)
      {
        var user = new ApplicationUser
        {
          UserName = "waqar@habib.com",
          Email = "waqar@habib.com",
          EmailConfirmed = true,
        };

        var result = await _userManager.CreateAsync(user, "Admin@123");
        if (result.Succeeded)
        {
          await _userManager.AddToRoleAsync(user, "Admin");
        }
      }

    }
  }
}
