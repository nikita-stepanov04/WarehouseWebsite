using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Web.Identity
{
    public static class IdentityServices
    {
        public static IServiceCollection SetUpIdentity(
            this IServiceCollection services, IConfiguration config)
        {
            JwtSettings jwtSettings = config.GetSection("Jwt").Get<JwtSettings>()!;
            services.AddIdentity<AppUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;

                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>();

            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,                    
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
                opts.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var tokenBlackList = context.HttpContext.RequestServices
                            .GetRequiredService<IMemoryCache>();
                        
                        var token = (context.SecurityToken as JsonWebToken)!.EncodedToken;
                        if (tokenBlackList.TryGetValue(token, out _))
                        {
                            context.Fail("Token is invalid");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(nameof(Policies.AdminsOnly), p => p.RequireRole(nameof(Roles.Admin)));
                opts.AddPolicy(nameof(Policies.UsersOnly), p => p.RequireRole(nameof(Roles.User)));
            });

            return services;
        }

        public static IServiceProvider SeedRoles(
           this IServiceProvider services)
        {
            SeedRolesAsync(services).Wait();
            return services;
        }

        private static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string user = nameof(Roles.User);
            string admin = nameof(Roles.Admin);

            if (!await roleManager.RoleExistsAsync(admin))
            {
                await roleManager.CreateAsync(new IdentityRole(admin));
            }

            if (!await roleManager.RoleExistsAsync(user))
            {
                await roleManager.CreateAsync(new IdentityRole(user));
            }
        }
    }
}
