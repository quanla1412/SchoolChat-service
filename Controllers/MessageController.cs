using Microsoft.AspNetCore.Mvc;
using SchoolChat.Service.Models;
using SchoolChat.Service.Service.ServiceImpl;

namespace SchoolChat.Service.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MessageController(IMessageService messageService) : ControllerBase
{
    [HttpGet]
    [ActionName("GetMessagesByChatRoomId")]
    public List<MessageViewModel> GetMessagesByChatRoomId([FromQuery] string chatRoomId)
    {
        return messageService.GetMessagesByChatRoomId(chatRoomId);
    }
    
}