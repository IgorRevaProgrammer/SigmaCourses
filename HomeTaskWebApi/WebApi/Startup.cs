using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Models.Models;
using Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApi.Areas.Identity.Data;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<CourseService>();
            services.AddScoped<HomeTaskService>();
            services.AddScoped<StudentService>();

            services.AddDbContext<UniversityContext>(
            options => options.UseSqlServer(connectionString));

            services.AddScoped<IRepository<Student>, StudentRepository>();
            services.AddScoped<IRepository<Course>, CourseRepository>();
            services.AddScoped<IRepository<HomeTask>, HomeTaskRepository>();
            services.AddScoped<IRepository<HomeTaskAssessment>, HomeTaskAssessmentRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Home}/{id?}");
                endpoints.MapRazorPages();
            });

            CreateAdminUser(userManager, roleManager).Wait();
        }

        private async Task CreateAdminUser(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            User identityUser = new User
            {
                Email = "Admin@test.com",
                UserName = "Admin@test.com",
                Name = "Admin",
                DateOfBirth = System.DateTime.Now
            };
            var userRes = await userManager.CreateAsync(identityUser, "admin1234#Q");
            var rol = await roleManager.CreateAsync(new IdentityRole("Admin"));
            var res = await userManager.AddToRoleAsync(identityUser, "Admin");
            var isInRole = await userManager.IsInRoleAsync(identityUser, "Admin");
            var roles = await userManager.GetRolesAsync(identityUser);
            var allRoles = await roleManager.Roles.ToListAsync();
            var x = userManager.Options.SignIn.RequireConfirmedAccount;
        }
    }
}