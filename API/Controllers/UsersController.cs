using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _PhotoService;

    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService PhotoService)
    {
        _PhotoService = PhotoService;
        _userRepository = userRepository;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams  userParams)
    {

        var currentUser = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        userParams.CurrentUerName = currentUser.UserName;

        if (string.IsNullOrEmpty(userParams.Gender))
        {
            userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
        }

        var users = await _userRepository.GetMembersAsync(userParams);

        Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages));

        return Ok(users);

    }

    [HttpGet("{username}")] // api/users/2
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        return await _userRepository.GetMemberAsync(username);

    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(memberUpdateDto memberUpdateDto)
    {

        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return NotFound();

        _mapper.Map(memberUpdateDto, user);
        if (await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failde to update user");
    }


    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return NotFound();

        var results = await _PhotoService.AddPhotoAsync(file);

        if (results.Error != null) return BadRequest(results.Error.Message);

        var photo = new Photo
        {
            Url = results.SecureUrl.AbsoluteUri,
            PublicId = results.PublicId
        };

        if (user.Photos.Count == 0) photo.isMain = true;

        user.Photos.Add(photo);

        if (await _userRepository.SaveAllAsync())
        {
            return CreatedAtAction
            (nameof(GetUser),
             new { username = user.UserName },
            _mapper.Map<PhotoDto>(photo)
            );
        };

        return BadRequest("Error while updating photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> setmainPhoto(int photoId)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return NotFound();

        var photo =     user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null) return NotFound();

        if(photo.isMain) return BadRequest("already has this main");

        var currentMain = user.Photos.FirstOrDefault(x=>x.isMain);
        if (currentMain != null)  currentMain.isMain = false;
        photo.isMain= true;;

        if(await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("problem while setting main photo");

    }


    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null) return NotFound();

        if(photo.isMain) return  BadRequest("cant delete main photo");

        if(photo.PublicId != null)
        {
            var results = await _PhotoService.DeletePhotoAsync(photo.PublicId);
            if (results.Error != null) return BadRequest(results.Error.Message);
        } 

        user.Photos.Remove(photo);

        if(await _userRepository.SaveAllAsync()) return Ok();

        return BadRequest("problem deleting photo");
       
    }


}
