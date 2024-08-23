using CRCQRS.API.DataSeeding;
using CRCQRS.Application.Commands.Handlers;
using CRCQRS.Domain;
using CRCQRS.DTO;
using CRCQRS.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace CRCQRS.API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

      builder.Services.AddCors(options =>
      {
        options.AddPolicy("AllowAngularApp",
            builder => builder
                .WithOrigins("http://localhost:4200") // URL of your Angular app
                .AllowAnyHeader()
                .AllowAnyMethod());
      });
      // Add services to the container.
      var connection = builder.Configuration.GetConnectionString("DBB");
      builder.Services.AddDbContext<CRCQRSContext>(options => options.UseSqlServer(connection));
      builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<CRCQRSContext>()
                .AddDefaultTokenProviders();
      builder.Services.AddScoped<IDataSeeder, DataSeeder>();
      builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommandHandler>());


      builder.Services.AddAuthorization();
      //builder.Services.AddAuthorization(options =>
      //{
      //  options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
      //  options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
      //  options.AddPolicy("VendorOnly", policy => policy.RequireRole("Vendor"));

      //  options.AddPolicy("AdminOrVendor", policy => policy.RequireRole("Admin", "Vendor"));
      //  options.AddPolicy("CustomerOrAdminOrVendor", policy => policy.RequireRole("Customer", "Admin", "Vendor"));

      //});

      builder.Services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();

      var app = builder.Build();
      InitializeDatabase(app);
      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();

      app.UseAuthentication();
      app.UseAuthorization();


      app.MapControllers();

      app.Run();
    }
    private static void InitializeDatabase(WebApplication app)
    {
      using (var scope = app.Services.CreateScope())
      {
        var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        var dbContext = scope.ServiceProvider.GetRequiredService<CRCQRSContext>();
        dbContext.Database.EnsureCreated();
        dataSeeder.SeedAsync().Wait();

      }
    }
  }
}
