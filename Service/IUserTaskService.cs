using SchoolChat.Service.Models;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service;

public interface IUserTaskService
{
    UserTaskViewModel GetById(string id);
    List<UserTaskViewModel> GetByChatRoomId(string chatRoomId);
    UserTaskViewModel Create(CreateTaskViewModel task);
    UserTaskViewModel Update(UserTaskViewModel task);
    Boolean Delete(string id);
}