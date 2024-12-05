using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolChat.Service.Models;
using SchoolChat.Service.Service;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ChatRoomController(UserManager<User> userManager, IChatRoomService chatRoomService) : ControllerBase
{
    [HttpGet]
    [ActionName("GetChatRooms")]
    public List<ChatRoomViewModel> GetChatRooms()
    {
        string currentUserId = userManager.GetUserId(User);
        List<ChatRoomViewModel> result = chatRoomService.GetChatRoomsByUserId(currentUserId);

        return result;
    }
    
    [HttpGet]
    [ActionName("Detail")]
    public ChatRoomDetailViewModel GetChatRooms([FromQuery] string id)
    {
        string currentUserId = userManager.GetUserId(User);
        ChatRoomDetailViewModel result = chatRoomService.GetChatRoomDetailById(id, currentUserId);

        return result;
    }
    
    [HttpPost]
    [ActionName("Create")]
    public IActionResult Create([FromBody] CreateChatRoomViewModel model)
    {
        model.FromUserId = userManager.GetUserId(User);
        ChatRoomViewModel result = chatRoomService.CreateChatRoom(model);
        
        return Ok(result);
    }
}