using E_Commerce_Mezzex.Models.Domain;
using System.Collections.Generic;

namespace E_Commerce_Mezzex.Models.ViewModel
{
    public class UserPermissionsViewModel
    {
        public IList<ApplicationUser> Users { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<UserPermission> UserPermissions { get; set; }
    }

}
