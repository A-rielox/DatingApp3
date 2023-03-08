using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(DataContext context,
                             ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // POST: api/Account/register
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExists(registerDto.Username)) return BadRequest("Username is taken");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };

        return Ok(userDto);
    }

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // POST: api/Account/login
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u =>
                    u.UserName == loginDto.Username);

        if (user == null) return Unauthorized("Invalid Username.");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password.");
        }

        var userDto = new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };

        return Ok(userDto);
    }




    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    //

    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(u => u.UserName == username.ToLower());
    }
}
