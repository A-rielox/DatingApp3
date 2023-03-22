using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(UserManager<AppUser> userManager,
                             ITokenService tokenService,
                             IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // POST: api/Account/register
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

        var user = _mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.Username.ToLower();

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, "Member");

        if(!roleResult.Succeeded) return BadRequest(roleResult.Errors);

        var userDto = new UserDto
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Gender = user.Gender,
            Token = await _tokenService.CreateToken(user)
        };

        return Ok(userDto);
    }

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // POST: api/Account/login
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
                                 .Include(u => u.Photos)
                                 .SingleOrDefaultAsync(u =>
                                    u.UserName == loginDto.Username);

        if (user == null) return Unauthorized("Invalid Username.");

        var result = await _userManager.CheckPasswordAsync(user,loginDto.Password);

        if (!result) return Unauthorized("Invalid Password.");

        var userDto = new UserDto
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Gender = user.Gender,
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
            Token = await _tokenService.CreateToken(user)
        };

        return Ok(userDto);
    }




    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    //

    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(u => u.UserName == username.ToLower());
    }
}
