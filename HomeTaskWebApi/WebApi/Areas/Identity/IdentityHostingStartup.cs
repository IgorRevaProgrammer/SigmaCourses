using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Areas.Identity.Data;
using WebApi.Authorization;

[assembly: HostingStartup(typeof(WebApi.Areas.Identity.IdentityHostingStartup))]
namespace WebApi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UserContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UserContextConnection")));
                services.AddDatabaseDeveloperPageExceptionFilter();
                
                services.AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 0;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.User.RequireUniqueEmail = true;
                }).AddRoles<IdentityRole>()
                  .AddEntityFrameworkStores<UserContext>();
                services.AddAuthentication().AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = "634679217715-npsogcl7nf9l8rohole0uku54g7gspcr.apps.googleusercontent.com";
                    googleOptions.ClientSecret = "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";
                });
                services.AddScoped<IAuthorizationHandler, UserAccessRequirementAuthorizationHandler>();
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("UserAccessPolicy", builder =>
                    {
                        builder.Requirements.Add(new UserAccessRequirement());
                    });
                });
            });
        }
    }
}