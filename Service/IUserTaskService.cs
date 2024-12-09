using SchoolChat.Service.Models;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service;

public interface IUserTaskService
{
    UserTaskViewModel GetById(string id);
    List<UserTaskViewModel> GetByChatRoomId(string chatRoomId);
    List<UserTaskViewModel> GetByUserId(string userId);
    UserTaskViewModel Create(CreateTaskViewModel task, string currentUserId);
    UserTaskViewModel Update(UserTaskViewModel task);
    Boolean Delete(string id);
    void CompleteTask(string taskId);
}