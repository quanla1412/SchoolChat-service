using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class UserTaskRepositoryImpl(ChatDbContext context) : IUserTaskRepository
{
    public UserTask? GetById(string id)
    {
        return context.UserTasks.FirstOrDefault(task => task.Id == id);
    }

    public List<UserTask> GetByChatRoomId(string roomId)
    {
        return context.UserTasks.Where(task => task.ChatRoomId == roomId).ToList();
    }

    public List<UserTask> GetByCreatorId(string creatorId)
    {
        return context.UserTasks.Where(task => task.CreatorId == creatorId).ToList();
    }

    public List<UserTask> GetByAssignToUserId(string userId)
    {
        var taskIds = context.TaskAssignees.Where(taskAssignee => taskAssignee.UserId == userId).Select(taskAssignee => taskAssignee.TaskId).ToList();
        return context.UserTasks.Where(task => taskIds.Contains(task.Id)).ToList();
    }

    public void Add(UserTask task)
    {
        context.UserTasks.Add(task);
        context.SaveChanges();
    }

    public void Update(UserTask task)
    {
        context.UserTasks.Update(task);
        context.SaveChanges();
    }

    public void Delete(string id)
    {
        UserTask task = context.UserTasks.FirstOrDefault(task => task.Id == id);
        
        if (task == null) throw new NullReferenceException();
        
        context.UserTasks.Remove(task);
        context.SaveChanges();
    }
}