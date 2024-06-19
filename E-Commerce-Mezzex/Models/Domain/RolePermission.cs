namespace E_Commerce_Mezzex.Models.Domain
{
    public class RolePermission
    {
        public string RoleId { get; set; }
        public int PermissionId { get; set; }

        public ApplicationRole Role { get; set; }
        public Permission Permission { get; set; }
    }
}
