using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace E_Commerce_Mezzex.Models.Domain
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
