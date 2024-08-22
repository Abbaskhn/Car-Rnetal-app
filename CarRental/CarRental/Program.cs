using application.Dbcontext;
using application.model;
using CarRental.Interface;
using CarRental.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Stripe settings
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

// Configure CORS to allow your Angular app
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAngularApp",
      builder => builder
          .WithOrigins("http://localhost:4200") // URL of your Angular app
          .AllowAnyHeader()
          .AllowAnyMethod());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the database connection
var connection = builder.Configuration.GetConnectionString("DBB");
builder.Services.AddDbContext<Dbuser>(options => options.UseSqlServer(connection));

// Configure Authentication and Authorization
builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.RequireHttpsMetadata = false;
  options.SaveToken = true;
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsSecretKeyForMyApplication0000099992222madebyzohaibali")),
    ValidateIssuer = false,
    ValidateAudience = false
  };
});

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
  options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
  options.AddPolicy("VendorOnly", policy => policy.RequireRole("Vendor"));

    options.AddPolicy("AdminOrVendor", policy => policy.RequireRole("Admin", "Vendor"));
    options.AddPolicy("CustomerOrAdminOrVendor", policy => policy.RequireRole("Customer", "Admin", "Vendor"));
 
});

// Register application services
builder.Services.AddScoped<IFileService, CarRental.Services.FileService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAngularApp");
 // This allows serving static files like images from wwwroot

// Static files middleware for serving uploaded images


// Add authentication and authorization middlewares
app.UseAuthentication();
app.UseAuthorization();

// Map the controllers
app.MapControllers();

app.Run();
