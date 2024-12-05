using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository;

public interface ITaskAssigneeRepository
{
    TaskAssignee? GetById(string id);
    List<TaskAssignee> GetByTaskId(string taskId);
    void Add(TaskAssignee taskAssignee);
    void Delete(string id);
    void DeleteByTaskId(string taskId);
}