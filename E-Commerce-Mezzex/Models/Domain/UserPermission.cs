namespace E_Commerce_Mezzex.Models.Domain
{
    public class UserPermission
    {
        public string UserId { get; set; }
        public int PermissionId { get; set; }

        public ApplicationUser User { get; set; }
        public Permission Permission { get; set; }
    }
}
