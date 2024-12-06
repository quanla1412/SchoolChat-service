using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolChat.Service.Models;
using SchoolChat.Service.Service;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController(UserManager<User> userManager, IUserService userService) : ControllerBase
{
    [HttpGet]
    [ActionName("GetCurrentUser")]
    public UserViewModel GetCurrentUser()
    {
        return userService.GetUserById(userManager.GetUserId(User));
    }
    
    [HttpGet]
    [ActionName("GetUsers")]
    public IEnumerable<UserViewModel> GetUsers([FromQuery] string searchString = "", [FromQuery] bool excludeCurrentUser = false)
    {
        return userService.GetUsers(searchString, excludeCurrentUser ? userManager.GetUserId(User) : null);
    }

    [HttpPost]
    [ActionName("UpdateProfile")]
    public IActionResult UpdateProfile([FromForm] UpdateProfileViewModel profileViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            var updatedUser = userService.UpdateProfile(profileViewModel);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while updating the profile.",
                details = ex.Message
            });
        }
    }
    /*[HttpPost(Name = "SignUp")]
    public Profile SignUp([FromBody] RegisterViewModel model)
    {
        _accountService

        return _dbContext.ChatRooms.ToArray();
    }*/
}