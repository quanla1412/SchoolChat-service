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
    
    [HttpPost("Forward")]
    public ActionResult<Message> ForwardMessage([FromBody] ForwardMessageModel model)
    {
        try
        {
            string currentUserId = userManager.GetUserId(User);
            var forwardedMessage = messageService.ForwardMessage(model, currentUserId);
            return Ok(forwardedMessage);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    [ActionName("Pin")]
    public ActionResult<Message> PinMessage([FromQuery] string messageId)
    {
        try
        {
            var pinnedMessage = messageService.PinMessage(messageId);
            return Ok(pinnedMessage);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("Unsent")]
    public ActionResult<Message> UnsentMessage([FromQuery] string messageId)
    {
        bool success = messageService.UnsentMessage(messageId);
        return success ? Ok() : NotFound();
    }
    
    [HttpPost("Delete")]
    public ActionResult<Boolean> DeleteMessage([FromBody] string messageId)
    {
        try
        {
            string currentUserId = userManager.GetUserId(User);
            var isDeleted = messageService.DeleteMessage(messageId, currentUserId);
            if (isDeleted)
            {
                return Ok(true);
            }
            else
            {
                return NotFound("Message not found");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}