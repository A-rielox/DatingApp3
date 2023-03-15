using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository,
                           IMapper mapper,
                           IPhotoService photoService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _photoService = photoService;
    }


    ////////////////////////////////////////
    ////////////////////////////////////////
    //      GET: api/users/
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userRepository.GetMembersAsync();

        return Ok(users);
    }


    ////////////////////////////////////////
    ////////////////////////////////////////
    //      GET: api/users/username
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await _userRepository.GetMemberAsync(username);

        return Ok(user);
    }


    ////////////////////////////////////////
    ////////////////////////////////////////
    //      PUT: api/users
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.GetUsername();

        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null) return NotFound();

        _mapper.Map(memberUpdateDto, user);

        if (await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");
    }


    ////////////////////////////////////////
    ////////////////////////////////////////
    //      POST: api/users/add-photo
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return NotFound();

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };

        if (user.Photos.Count == 0) photo.IsMain = true;

        user.Photos.Add(photo);

        if (await _userRepository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetUser),
                                   new { username = user.UserName },
                                   _mapper.Map<PhotoDto>(photo));
        }

        return BadRequest("Problem adding photo.");
    }


    ////////////////////////////////////////
    ////////////////////////////////////////
    //      PUT: api/users/set-main-photo/{photoId}
    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return NotFound();

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("This is already your main photo.");

        var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);

        if (currentMain != null) currentMain.IsMain = false;

        photo.IsMain = true;

        if (await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Problem setting the main photo");
    }

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // DELETE: api/Users/delete-photo/{photoId}
    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        // saco el usernane del token
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("You can not delete your main photo.");

        // si esta en cloudinary => tiene una publicId
        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (await _userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Failed to delete the photo.");
    }
}
