using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppRole : IdentityRole<int>
{
    // es la misma navigation-property hacia la join-table en AppUser.cs y AppRole.cs
    public ICollection<AppUserRole> UserRoles { get; set; }
}

// relacion many-to-many
// cada role puede tener many users
// cada user puede tener many roles
