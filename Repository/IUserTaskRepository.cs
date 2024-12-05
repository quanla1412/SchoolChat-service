using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository;

public interface IUserTaskRepository
{
    UserTask? GetById(string id);
    List<UserTask> GetByChatRoomId(string id);
    void Add(UserTask task);
    void Update(UserTask task);
    void Delete(string id);
}