using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository;

public interface IChatRoomRepository
{
    void Add(ChatRoom chatRoom);
}