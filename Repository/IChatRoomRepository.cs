using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository;

public interface IChatRoomRepository
{
    List<ChatRoom> GetChatRoomsByUserId(string userId);
    
    ChatRoom? GetChatRoomByUsers(string fromUserId, string toUserId);
    
    void Add(ChatRoom chatRoom);
}