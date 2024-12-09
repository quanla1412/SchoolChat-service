using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolChat.Service.Models;
using SchoolChat.Service.Service;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserTaskController(
    UserManager<User> userManager, 
    IUserTaskService userTaskService
    ) : ControllerBase
{
    [HttpGet]
    [ActionName("GetByChatRoom")]
    public ActionResult<List<UserTaskViewModel>> GetByChatRoomId(string chatRoomId)
    {
        List<UserTaskViewModel> tasks = userTaskService.GetByChatRoomId(chatRoomId);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    [ActionName("GetById")]
    public ActionResult<UserTaskViewModel> GetById(string id)
    {
        try
        {
            UserTaskViewModel task = userTaskService.GetById(id);
            return Ok(task);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Task not found.");
        }
    }
    
    [HttpGet]
    [ActionName("GetByUserId")]
    public ActionResult<List<UserTaskViewModel>> GetByUserId(string userId)
    {
        List<UserTaskViewModel> tasks = userTaskService.GetByUserId(userId);
        return Ok(tasks);
    }
    
    [HttpGet]
    [ActionName("CompleteTask")]
    public IActionResult CompleteTask(string taskId)
    {
        userTaskService.CompleteTask(taskId);
        return Ok();
    }

    [HttpPost]
    [ActionName("Create")]
    public ActionResult<UserTaskViewModel> Create([FromBody] CreateTaskViewModel task)
    {
        string currentUserId = userManager.GetUserId(HttpContext.User);
        UserTaskViewModel createdTask = userTaskService.Create(task, currentUserId);
        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }
    
    [HttpPost]
    [ActionName("Update")]
    public ActionResult<UserTaskViewModel> Update([FromBody] UserTaskViewModel task)
    {
        try
        {
            UserTaskViewModel updatedTask = userTaskService.Update(task);
            return Ok(updatedTask);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Task not found.");
        }
    }

    [HttpPost("{id}")]
    [ActionName("Delete")]
    public ActionResult<bool> Delete(string id)
    {
        var result = userTaskService.Delete(id);
        if (result)
        {
            return NoContent();
        }
        return NotFound("Task not found.");
    }
}