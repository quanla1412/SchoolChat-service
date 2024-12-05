using Microsoft.AspNetCore.Mvc;
using SchoolChat.Service.Service;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserTaskController(IUserTaskService userTaskService) : ControllerBase
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

    [HttpPost]
    [ActionName("Create")]
    public ActionResult<UserTaskViewModel> Create([FromBody] CreateTaskViewModel task)
    {
        UserTaskViewModel createdTask = userTaskService.Create(task);
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