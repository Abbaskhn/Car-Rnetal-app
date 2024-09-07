using AppCOG.Application.Commands;
using AppCOG.Application.Queries.Handlers;
using CRCQRS.API.DataSeeding;
using CRCQRS.Application.Commands;
using CRCQRS.Application.Commands.Handlers;
using CRCQRS.Application.Queries;
using CRCQRS.Application.Services;
using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.DTO;
using CRCQRS.Infrastructure;
using CRCQRS.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
namespace CRCQRS.API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      builder.Services.Configure<CRCQRS.Common.StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

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
      // builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommandHandler>());

      builder.Services.AddTransient<IRequestHandler<ChargeUserCommand, ResponseResult>, ChargeUserCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<DeleteBookingCommand, ResponseResult>, DeleteBookingCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<DeleteCarCommand, ResponseResult>, DeleteCarCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<DeleteUserCommand, ResponseResult>, DeleteUserCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<DeleteVendorCommand, ResponseResult>, DeleteVendorCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<LoginUserCommand, ResponseResult>, LoginUserCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<RegisterBookingCommand, ResponseResult>, RegisterBookingCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<RegisterCarCommand, ResponseResult>, RegisterCarCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<RegisterUserCommand, ResponseResult>, RegisterUserCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<RegisterVendorCommand, ResponseResult>, RegisterVendorCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<UpdateBookingCommand, ResponseResult>, UpdateBookingCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<UpdateCarCommand, ResponseResult>, UpdateCarCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<UpdateFileCommand, ResponseResult>, UpdateFileCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<UpdateUserCommand, ResponseResult>, UpdateUserCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<UpdateVendorCommand, ResponseResult>, UpdateVendorCommandHandler>();
      builder.Services.AddTransient<IRequestHandler<UploadFileCommand, ResponseResult>, UploadFileCommandHandler>();



      builder.Services.AddTransient<IRequestHandler<GetAllBookingsQuery, ResponseResult >, GetAllBookingQueryHandler>();
      builder.Services.AddTransient<IRequestHandler<GetAllCarsQuery, ResponseResult>, GetAllCarsQueryHandler>();
      builder.Services.AddTransient<IRequestHandler<GetAllUserQuery, ResponseResult>, GetAllUserQueryHandler>();
      builder.Services.AddTransient<IRequestHandler<GetAllVendorQuery, ResponseResult>, GetAllVendorQueryHandler>();
      builder.Services.AddTransient<IRequestHandler<GetBookingByIdQuery, ResponseResult>, GetBookingByIdQueryHandler>();
      builder.Services.AddTransient<IRequestHandler<GetCarsByIdQuery, ResponseResult>, GetCarsByIdCarsQueryHandler>();
      builder.Services.AddTransient<IRequestHandler<GetUserByIdQuery, ResponseResult>, GetUserByIdCarsQueryHandler>();
      builder.Services.AddTransient<IRequestHandler<GetVendorByIdQuery, ResponseResult>, GetVendorByIdCarsQueryHandler>();














      builder.Services.AddScoped<IUserInfoService, UserInfoService>();
      builder.Services.AddScoped<IStripeService, StripeService>();
      // builder.Services.AddAuthentication();
      builder.Services.AddAuthorization();
      //builder.Services.AddAuthorization(options =>
      //{
      //  options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
      //  options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
      //  options.AddPolicy("VendorOnly", policy => policy.RequireRole("Vendor"));

      //  options.AddPolicy("AdminOrVendor", policy => policy.RequireRole("Admin", "Vendor"));
      //  options.AddPolicy("CustomerOrAdminOrVendor", policy => policy.RequireRole("Customer", "Admin", "Vendor"));

      //});

      builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
      options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      //builder.Services.AddSwaggerGen();
      builder.Services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        // Define the security scheme for Bearer Authentication
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                          "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                          "Example: \"Bearer 12345abcdef\"",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer"
        });

        // Define the security requirement for all endpoints
        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
      });
      var app = builder.Build();
      InitializeDatabase(app);
      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }
      app.UseCors("AllowAngularApp");
      app.UseDeveloperExceptionPage();
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
