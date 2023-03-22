using API.Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // deserialize xq viene como json el userData y lo quiero pasar a <List<AppUser>>
        //var users = JsonSerializer.Deserialize<List<AppUser>>(userData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);// The solution was to change from System.Text.Json to Newtonsoft Json with this line

        //if (users == null) return;

        var roles = new List<AppRole>
        {
            new AppRole{Name = "Member"},
            new AppRole{Name = "Admin"},
            new AppRole{Name = "Moderator"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {

            user.UserName = user.UserName.ToLower();

            await userManager.CreateAsync(user, "P@ssword1"); // este crea y salva el cambio
            await userManager.AddToRoleAsync(user, "Member");
        }

        // p' crear el usuario Admin
        var admin = new AppUser
        {
            UserName = "admin"
        };
        await userManager.CreateAsync(admin, "P@ssword1");
        await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
    }
}




/*
public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);// The solution was to change from System.Text.Json to Newtonsoft Json with this line

        foreach (var user in users)
        {
            user.UserName = user.UserName.ToLower();

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}
*/
