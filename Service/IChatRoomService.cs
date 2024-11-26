using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service;

public interface IChatRoomService
{
    List<ChatRoomViewModel> GetChatRoomsByUserId(string userId);
    ChatRoomDetailViewModel? GetChatRoomById(string id);
    ChatRoomViewModel CreateChatRoom(CreateChatRoomViewModel model);
}