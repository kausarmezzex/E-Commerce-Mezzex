using E_Commerce_Mezzex.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CompanyName { get; set; }
    public bool IsTaxExempt { get; set; }
    public bool Active { get; set; }

    // Add this property
    public ICollection<UserPermission> UserPermissions { get; set; }
}
