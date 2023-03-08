namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string UserName { get; set; }

    // public byte[] PasswordHash { get; set; }
    //public byte[] PasswordSalt { get; set; }
    //public DateOnly DateOfBirth { get; set; }
    //public string KnownAs { get; set; }
    //public DateTime Created { get; set; } = DateTime.UtcNow;
    //public DateTime LastActive { get; set; } = DateTime.UtcNow;
    //public string Gender { get; set; }
    //public string Introduction { get; set; }
    //public string LookingFor { get; set; }
    //public string Interests { get; set; }
    //public string City { get; set; }
    //public string Country { get; set; }

    // relacion one-to-many ( un user many photos )
    //public List<Photo> Photos { get; set; } = new();
    // el Photo hago
    // p'q ocupe la id del AppUser como foreign-key
    // public AppUser AppUser { get; set; }
    // public int AppUserId { get; set; }
    // asi las fotos quedan ligadas a un AppUser, y cuando se borre un user se van a borrar las fotos
    // el cascade delete

    ////////////////////////////
    //public List<UserLike> LikedByUsers { get; set; } // los q te dan like
    //public List<UserLike> LikedUsers { get; set; } // a quienes les doy like

    // un SourceUser puede tener varios LikedUsers
    // un LikedUser puede tener varios LikedByUsers
    // la configuracion p' 2waybinding se hace en LA TABLA EN DataContext.cs

    ////////////////////////////

    //public List<Message> MessagesSent { get; set; }
    //public List<Message> MessagesReceived { get; set; }

    ////////////////////////////
    // es la misma navigation-property hacia la join-table en AppUser.cs y AppRole.cs
    //public ICollection<AppUserRole> UserRoles { get; set; }




    //
    //
    //
    // NO lo puedo incluir para q funcione el :
    // .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) en UserRepository
    // q es p' hacer + eficiente la busqueda en la db
    //
    //public int GetAge()
    //{
    //    return DateOfBirth.CalculateAge();
    //}
}
