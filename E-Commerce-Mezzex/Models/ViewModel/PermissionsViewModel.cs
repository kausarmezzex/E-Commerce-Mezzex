using E_Commerce_Mezzex.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace E_Commerce_Mezzex.Models.ViewModel
{
    public class PermissionsViewModel
    {
        public List<ApplicationRole> Roles { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<UserPermission> UserPermissions { get; set; }
        public string SelectedRoleName { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}
