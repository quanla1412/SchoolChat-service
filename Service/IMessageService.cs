using SchoolChat.Service.Models;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service;

public interface IMessageService
{
    List<MessageViewModel> GetMessagesByChatRoomId(string chatRoomId);
    MessageViewModel? GetNewestMessagesByChatRoomId(string chatRoomId);
    PinnedMessageViewModel? GetPinnedMessagesByChatRoomId(string chatRoomId);
    ReadMessageStatusViewModel MarkReadMessage(string messageId, string userId);
    void MarkReadMessageByChatRoomId(string chatRoomId, string userId);
    MessageViewModel Add(CreateMessageViewModel message);
    MessageViewModel? ForwardMessage(ForwardMessageModel message, string currentUserId);
    PinnedMessageViewModel? PinMessage(string messageId);
    void UnpinMessage(string messageId);
    bool UnsentMessage(string messageId);
    Boolean DeleteMessage(string messageId, string currentUserId);
    string? UploadFile(IFormFile file);
}