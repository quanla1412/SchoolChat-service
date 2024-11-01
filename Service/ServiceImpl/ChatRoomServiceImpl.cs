using SchoolChat.Service.Models;
using SchoolChat.Service.Repository;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public class ChatRoomServiceImpl : IChatRoomService
{
    private readonly IUserRepository _userRepository;
    private readonly IChatRoomRepository _chatRoomRepository;

    public ChatRoomServiceImpl(IUserRepository userRepository, IChatRoomRepository chatRoomRepository)
    {
        _userRepository = userRepository;
        _chatRoomRepository = chatRoomRepository;
    }
    
    public ChatRoomViewModel CreateChatRoom(string fromUserId, string toUserId)
    {
        List<ChatRoomUser> chatRoomUsers = new List<ChatRoomUser>();
        User? fromUser = _userRepository.GetUserById(fromUserId);
        User? toUser = _userRepository.GetUserById(toUserId);
        if (fromUser == null || toUser == null) 
            throw new Exception("User not found");
        
        chatRoomUsers.Add(new ChatRoomUser()
        {
            Id = Guid.NewGuid().ToString(),
            User = fromUser
        });
        
        chatRoomUsers.Add(new ChatRoomUser()
        {
            Id = Guid.NewGuid().ToString(),
            User = toUser
        });

        ChatRoom chatRoom = new ChatRoom()
        {
            Id = Guid.NewGuid().ToString(),
            Users = chatRoomUsers
        };
        
        _chatRoomRepository.Add(chatRoom);
        
        return new ChatRoomViewModel()
        {
            Id = chatRoom.Id,
            Name = chatRoom.Name
        };
    }
}