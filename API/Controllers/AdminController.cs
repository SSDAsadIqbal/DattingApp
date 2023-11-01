using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        //<-------------------------------------------------------------------- [HttpGet("users-with-roles")]----------------------------------------------------------------------------------->

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUserWithRoles()
        {
                var users = await _userManager.Users
                    .OrderBy(u => u.UserName)
                    .Select(u => new
                    {
                        u.Id,
                        Username = u.UserName,
                        Roles = u.userRoles.Select(r => r.Role.Name).ToList()
                    })
                    .ToListAsync();

                return Ok(users);
        }

        //<-------------------------------------------------------------------- [HttpPost("edit-roles/{username}")]----------------------------------------------------------------------------------->

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery]string roles)
        {
            if(string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

            var selectedRole = roles.Split(",").ToArray();

            var user = await  _userManager.FindByNameAsync(username);

            if(user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRole.Except(userRoles));

            if(!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user , userRoles.Except(selectedRole));

            if(!result.Succeeded) return BadRequest("Failed to remove to roles");

            return Ok((await _userManager.GetRolesAsync(user)));
        }

        //<-------------------------------------------------------------------- [HttpGet("photos-to-moderate")]----------------------------------------------------------------------------------->

        [Authorize(Policy = "ModeratorPhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Only Amin or Moderator Can see this");
        }
    }
}