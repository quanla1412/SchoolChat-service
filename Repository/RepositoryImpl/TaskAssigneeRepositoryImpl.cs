using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class TaskAssigneeRepositoryImpl(ChatDbContext context) : ITaskAssigneeRepository
{
    public TaskAssignee? GetById(string id)
    {
        return context.TaskAssignees.FirstOrDefault(task => task.Id == id);
    }

    public List<TaskAssignee> GetByTaskId(string taskId)
    {
        return context.TaskAssignees.Where(ta => ta.TaskId == taskId).ToList();
    }

    public void Add(TaskAssignee taskAssignee)
    {
        context.TaskAssignees.Add(taskAssignee);
        context.SaveChanges();
    }
    
    public void Delete(string id)
    {
        var assignee = GetById(id);
        if (assignee != null)
        {
            context.TaskAssignees.Remove(assignee);
            context.SaveChanges();
        }
    }
    
    public void DeleteByTaskId(string taskId)
    {
        var assignees = GetByTaskId(taskId);
        context.TaskAssignees.RemoveRange(assignees);
        context.SaveChanges();
    }
}