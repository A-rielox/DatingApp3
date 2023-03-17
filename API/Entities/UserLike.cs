namespace API.Entities;

// ESTO ES UN LIKE CON EL ID DE QUIEN LO DA Y QUIEN LO RECIBE

public class UserLike
{
    // CONFIGURO LA TABLA EN DataContext.cs
    public AppUser SourceUser { get; set; } // el q da el like
    public int SourceUserId { get; set; } // id del q da le like


    public AppUser TargetUser { get; set; } // al q se le da el like
    public int TargetUserId { get; set; }
}

// un SourceUser puede tener varios LikedUsers
// un LikedUser puede tener varios LikedByUsers
