using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository;

public interface IUserTaskRepository
{
    UserTask? GetById(string id);
    List<UserTask> GetByChatRoomId(string id);
    List<UserTask> GetByCreatorId(string creatorId);
    List<UserTask> GetByAssignToUserId(string creatorId);
    void Add(UserTask task);
    void Update(UserTask task);
    void Delete(string id);
}