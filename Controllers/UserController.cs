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
    public IEnumerable<UserViewModel> GetUsers([FromQuery] string searchString = "")
    {
        return userService.GetUsers(searchString);
    }
    
    /*[HttpPost(Name = "SignUp")]
    public Profile SignUp([FromBody] RegisterViewModel model)
    {
        _accountService

        return _dbContext.ChatRooms.ToArray();
    }*/
}