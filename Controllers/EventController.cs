using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolChat.Service.Models;
using SchoolChat.Service.Service;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class EventController(
    UserManager<User> userManager,
    IEventService eventService
    ) : ControllerBase
{
    [HttpGet]
    [ActionName("GetEvents")]
    public IActionResult GetEvents([FromQuery] bool isForAppCalendar = false)
    {
        string currentUserId = userManager.GetUserId(User);
        if (isForAppCalendar)
        {
            List<ShortEventViewModel> result = eventService.GetShortEvents(currentUserId);
            return Ok(result);
        }

        return Ok();
    }
    
    [HttpPost]
    [ActionName("Create")]
    public IActionResult Create([FromBody] CreateEventViewModel model)
    {
        string currentUserId = userManager.GetUserId(User);
        EventDetailViewModel result = eventService.Create(model, currentUserId);
        
        return Ok(result);
    }
}