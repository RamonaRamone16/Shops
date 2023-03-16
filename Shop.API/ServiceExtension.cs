using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shop.BLL.Services;
using Shop.DAL;
using Shop.DAL.Seeds;
using Shop.Models.Entities;
using System.Text;
using System.Text.Json.Serialization;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace Shop.API
{
    public static class ServiceExtension
    {
        public static void ConfigureSqlContext(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<ApplicationDbContext>(
                o => o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Shop.DAL")));
        }

        public static void ConfigureIdentity(this IServiceCollection service) 
        {
            var builder = service.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection service, IConfiguration configuration) 
        {
            var jwtConfig = configuration.GetSection("JwtConfig");
            var secretKey = jwtConfig["secret"];
            service.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o => 
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,  
                    ValidateAudience = true,    
                    ValidateLifetime = true,    
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["validIssuer"],
                    ValidAudience = jwtConfig["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services) 
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Shop API",
                    Version = "v1",
                    Description = "Shop API"
                });
                s.ResolveConflictingActions(apiDescription => apiDescription.First());
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT authorization using Bearer scheme"
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        { 
                            Reference = new OpenApiReference
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        public async static Task ConfigureSeedDataAsync(this IServiceCollection services) 
        {
            var scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                await ApplicationDbInitializer.SeedDataAsync(userManager, roleManager);
            }
        }

        public static void ConfigureJsonOptions(this IServiceCollection services)
        {
            services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.Configure<MvcJsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        }

        public static void ConfigureServices(this IServiceCollection service) 
        {
            service.AddScoped<AuthService>();
        }
    }
}
