using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityUserRoles_CoreMVC.Models
{
    public class RoleViewModel
    {
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        public List<SelectListItem> Users { get; set; }

        public List<SelectListItem> Roles { get; set; }


    }
}
