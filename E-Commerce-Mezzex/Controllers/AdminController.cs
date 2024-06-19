using E_Commerce_Mezzex.Models.Domain;
using E_Commerce_Mezzex.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Mezzex.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // Method to manage role permissions
        public async Task<IActionResult> ManageRolePermissions()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var allClaims = await _context.Permissions.ToListAsync();

            var model = new PermissionsViewModel
            {
                Roles = roles,
                Permissions = allClaims,
                RolePermissions = await _context.RolePermissions.ToListAsync(),
                IsSuperAdmin = User.IsInRole("SuperAdmin")
            };

            return View(model);
        }

        // POST method to update role permissions
        [HttpPost]
        public async Task<IActionResult> UpdateRolePermissions(Dictionary<string, Dictionary<int, bool>> permissions)
        {
            foreach (var roleEntry in permissions)
            {
                var roleId = roleEntry.Key;
                var role = await _roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    return NotFound();
                }

                foreach (var permissionEntry in roleEntry.Value)
                {
                    var permissionId = permissionEntry.Key;
                    var isAssigned = permissionEntry.Value;

                    var rolePermission = await _context.RolePermissions
                        .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

                    if (isAssigned && rolePermission == null)
                    {
                        var newRolePermission = new RolePermission { RoleId = roleId, PermissionId = permissionId };
                        _context.RolePermissions.Add(newRolePermission);
                    }
                    else if (!isAssigned && rolePermission != null)
                    {
                        _context.RolePermissions.Remove(rolePermission);
                    }
                }

                await _context.SaveChangesAsync();

                // Propagate permissions to all users in the role
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                foreach (var user in usersInRole)
                {
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    foreach (var permissionEntry in roleEntry.Value)
                    {
                        var permissionId = permissionEntry.Key;
                        var isAssigned = permissionEntry.Value;

                        var permission = await _context.Permissions.FindAsync(permissionId);

                        if (permission == null)
                        {
                            continue;
                        }

                        var hasClaim = userClaims.Any(c => c.Type == "Permission" && c.Value == permission.Name);

                        if (isAssigned && !hasClaim)
                        {
                            var claim = new System.Security.Claims.Claim("Permission", permission.Name);
                            await _userManager.AddClaimAsync(user, claim);
                        }
                        else if (!isAssigned && hasClaim)
                        {
                            var claimToRemove = userClaims.FirstOrDefault(c => c.Type == "Permission" && c.Value == permission.Name);
                            if (claimToRemove != null)
                            {
                                await _userManager.RemoveClaimAsync(user, claimToRemove);
                            }
                        }
                    }
                }
            }

            return RedirectToAction("ManageRolePermissions");
        }

        public async Task<IActionResult> ManageUserPermissions(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound();
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            var allClaims = await _context.Permissions.ToListAsync();

            var model = new PermissionsViewModel
            {
                Roles = new List<ApplicationRole> { role },
                Users = usersInRole.ToList(),
                Permissions = allClaims,
                UserPermissions = await _context.UserPermissions.ToListAsync(),
                SelectedRoleName = roleName
            };

            foreach (var user in usersInRole)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                foreach (var claim in userClaims)
                {
                    if (claim.Type == "Permission")
                    {
                        var permission = model.Permissions.FirstOrDefault(p => p.Name == claim.Value);
                        if (permission != null)
                        {
                            model.UserPermissions.Add(new UserPermission
                            {
                                UserId = user.Id,
                                PermissionId = permission.Id
                            });
                        }
                    }
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserPermissions(Dictionary<string, Dictionary<int, bool>> permissions, string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is required.");
            }

            foreach (var userEntry in permissions)
            {
                var userId = userEntry.Key;
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                foreach (var permissionEntry in userEntry.Value)
                {
                    var permissionId = permissionEntry.Key;
                    var isAssigned = permissionEntry.Value;

                    var permissionName = _context.UserClaims
                        .Where(c => c.ClaimType == "Permission")
                        .Select(c => c.ClaimValue)
                        .Distinct()
                        .Skip(permissionId - 1)
                        .FirstOrDefault();

                    if (permissionName == null)
                    {
                        continue;
                    }

                    var userClaims = await _userManager.GetClaimsAsync(user);
                    var hasClaim = userClaims.Any(c => c.Type == "Permission" && c.Value == permissionName);

                    if (isAssigned && !hasClaim)
                    {
                        var claim = new System.Security.Claims.Claim("Permission", permissionName);
                        await _userManager.AddClaimAsync(user, claim);
                    }
                    else if (!isAssigned && hasClaim)
                    {
                        var claimToRemove = userClaims.FirstOrDefault(c => c.Type == "Permission" && c.Value == permissionName);
                        if (claimToRemove != null)
                        {
                            await _userManager.RemoveClaimAsync(user, claimToRemove);
                        }
                    }
                }
            }

            return RedirectToAction("ManageUserPermissions", new { roleName });
        }

    }
}
