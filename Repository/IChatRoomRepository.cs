using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository;

public interface IChatRoomRepository
{
    ChatRoom? GetChatRoomById(string id);
    
    List<ChatRoom> GetChatRoomsByUserId(string userId);
    
    ChatRoom? GetChatRoomByUsers(string fromUserId, List<string> toUserIds);
    
    void Add(ChatRoom chatRoom);
    
    void Update(ChatRoom chatRoom);

    void AddChatRoomUser(List<ChatRoomUser> chatRoomUsers);
}