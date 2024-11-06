using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service;

public interface IChatRoomService
{
    List<ChatRoomViewModel> GetChatRoomsByUserId(string userId);
    ChatRoomViewModel CreateChatRoom(string fromUserId, string toUserId);
}