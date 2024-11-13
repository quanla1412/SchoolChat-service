using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolChat.Service.Models;
using SchoolChat.Service.Service.ServiceImpl;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MessageController(UserManager<User> userManager, IMessageService messageService) : ControllerBase
{
    [HttpGet]
    [ActionName("GetMessagesByChatRoomId")]
    public List<MessageViewModel> GetMessagesByChatRoomId([FromQuery] string chatRoomId)
    {
        return messageService.GetMessagesByChatRoomId(chatRoomId);
    }
    
    [HttpGet]
    [ActionName("MarkReadMessage")]
    public ReadMessageStatusViewModel MarkReadMessage([FromQuery] string messageId)
    {
        return messageService.MarkReadMessage(messageId, userManager.GetUserId(User));
    }
}