using System.Collections.Generic;

namespace E_Commerce_Mezzex.Models.Domain
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserPermission> UserPermissions { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
