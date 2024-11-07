using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository;

public interface IMessageRepository
{
    List<Message> GetMessagesByChatRoomId(string chatRoomId);
    Message? GetNewestMessagesByChatRoomId(string chatRoomId);
    void Add(Message message);
}