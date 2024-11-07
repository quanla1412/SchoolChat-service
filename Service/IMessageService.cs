using SchoolChat.Service.Models;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public interface IMessageService
{
    List<MessageViewModel> GetMessagesByChatRoomId(string chatRoomId);
    MessageViewModel? GetNewestMessagesByChatRoomId(string chatRoomId);
    MessageViewModel Add(CreateMessageViewModel message);
}