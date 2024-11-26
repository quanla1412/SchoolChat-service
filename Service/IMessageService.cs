using SchoolChat.Service.Models;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public interface IMessageService
{
    List<MessageViewModel> GetMessagesByChatRoomId(string chatRoomId);
    MessageViewModel? GetNewestMessagesByChatRoomId(string chatRoomId);
    ReadMessageStatusViewModel MarkReadMessage(string messageId, string userId);
    void MarkReadMessageByChatRoomId(string chatRoomId, string userId);
    MessageViewModel Add(CreateMessageViewModel message);
    Message ForwardMessage(ForwardMessageModel message);
    Message? PinMessage(string messageId);
    Message? UnsentMessage(string messageId, string currentUserId);
    Boolean DeleteMessage(string messageId, string currentUserId);
}