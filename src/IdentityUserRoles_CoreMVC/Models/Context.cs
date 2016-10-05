using IdentityUserRoles_CoreMVC.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityUserRoles_CoreMVC.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
    public class ApplicationUser : IdentityUser
    {
    }
    public static class SeedData
    {
        //app.Seed(); //Add This Code to Startup.cs Configure 

        public static async void Seed(this IApplicationBuilder app, UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            HomeController ctrl = new HomeController(userManager, context, roleManager);
           await ctrl.UseRoleManager();
        }
    }
}
