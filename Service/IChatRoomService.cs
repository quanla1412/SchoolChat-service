using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service;

public interface IChatRoomService
{
    List<ChatRoomViewModel> GetChatRoomsByUserId(string userId);
    ChatRoomDetailViewModel? GetChatRoomDetailById(string id, string currentUserId);
    ChatRoomViewModel CreateChatRoom(CreateChatRoomViewModel model);
}