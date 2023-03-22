using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(
                                        this IServiceCollection services,
                                        IConfiguration config
                                        )
    {
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false; // no es necesario pero na mas
        }).AddRoles<AppRole>()
          .AddRoleManager<RoleManager<AppRole>>()
          .AddEntityFrameworkStores<DataContext>(); // este es el q crea las tablas relacionadas a Identity





        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // esta y la sig es p'q revise q este firmado y revise la firma
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });



        //
        // CONFIFURACION  de las policies p' el acceso
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
        });

        return services;
    }
}
