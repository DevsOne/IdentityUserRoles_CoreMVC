using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityUserRoles_CoreMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityUserRoles_CoreMVC.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            RoleViewModel model = new RoleViewModel();
            List<SelectListItem> userList = new List<SelectListItem>();
            List<SelectListItem> roleList = new List<SelectListItem>();
            var users = _userManager.Users;
            var roles = _roleManager.Roles;
            foreach (var user in users)
            {
                userList.Add(new SelectListItem { Text = user.UserName, Value = user.UserName });
            }
            foreach (var role in roles)
            {
                roleList.Add(new SelectListItem { Text = role.Name, Value = role.Name });
            }
            model.Users = userList;
            model.Roles = roleList;

            return View(model);
        }

        public async Task UseContext()
        {
            List<IdentityRole> roles = new List<IdentityRole>();
            roles.Add(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            roles.Add(new IdentityRole { Name = "Senior", NormalizedName = "SENIOR" });
            roles.Add(new IdentityRole { Name = "Moderator", NormalizedName = "MODERATOR" });
            roles.Add(new IdentityRole { Name = "Member", NormalizedName = "MEMBER" });
            roles.Add(new IdentityRole { Name = "Junior", NormalizedName = "JUNIOR" });
            roles.Add(new IdentityRole { Name = "Candidate", NormalizedName = "CANDIDATE" });
            foreach (var role in roles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role.Name);
                if (!roleExist)
                {
                    _context.Add(role);
                }
            }
            _context.SaveChanges();

           await CreateUsers();
            var user = await _userManager.FindByIdAsync("1");
            await _userManager.AddToRoleAsync(user, "Admin");

        }
        public async Task UseRoleManager()
        {
            //Create Roles
            string[] roleNames = { "Admin", "Member", "Moderator", "Junior", "Senior", "Candidate" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Create Users
           await CreateUsers();

            //Assign Role To User(s)
            var user = await _userManager.FindByIdAsync("1");
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        public async Task CreateUsers()
        {
            List<ApplicationUser> newUsers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", Email = "one@email.com", UserName = "one" },
                new ApplicationUser { Id = "2", Email = "two@email.com", UserName = "two" },
                new ApplicationUser { Id = "3", Email = "three@email.com", UserName = "three" },
                new ApplicationUser { Id = "4", Email = "four@email.com", UserName = "four" },
                new ApplicationUser { Id = "5", Email = "five@email.com", UserName = "five" },
                new ApplicationUser { Id = "6", Email = "six@email.com", UserName = "six" },
                new ApplicationUser { Id = "7", Email = "seven@email.com", UserName = "seven" },
                new ApplicationUser { Id = "8", Email = "eight@email.com", UserName = "eight" },
                new ApplicationUser { Id = "9", Email = "nine@email.com", UserName = "nine" },
            };
            foreach (var user in newUsers)
            {
               await _userManager.CreateAsync(user, "12345Aa!");
            }
        }


        //Create Role From View
        [HttpPost]
        public async Task<ActionResult> CreateUserRole(RoleViewModel model)
        {
            IdentityResult roleResult;
            bool isRoleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!isRoleExists)
            {
                roleResult = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
            }

            //OR
            //_context.Add(new IdentityRole { Name = model.RoleName });
            //_context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        //Assign Role From View
        public async Task<ActionResult> AssignRole(RoleViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var roleCheck = await _userManager.IsInRoleAsync(user, model.RoleName);

            if (!roleCheck)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles != null)
                {
                    foreach (var role in userRoles)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role);
                    }
                }
                await _userManager.AddToRoleAsync(user, model.RoleName);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
