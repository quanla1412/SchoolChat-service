using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service;

public interface IChatRoomService
{
    ChatRoomViewModel CreateChatRoom(string fromUserId, string toUserId);
}