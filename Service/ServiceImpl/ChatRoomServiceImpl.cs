using SchoolChat.Service.Models;
using SchoolChat.Service.Repository;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public class ChatRoomServiceImpl(
    IUserRepository userRepository, 
    IChatRoomRepository chatRoomRepository,
    IMessageService messageService
    ) : IChatRoomService
{
    public List<ChatRoomViewModel> GetChatRoomsByUserId(string userId)
    {
        List<ChatRoomViewModel> result = new();
        List<ChatRoom> chatRooms = chatRoomRepository.GetChatRoomsByUserId(userId);
        
        foreach (var chatRoom in chatRooms)
        {
            List<UserViewModel> users = new();
            foreach (var chatRoomUser in chatRoom.Users)
            {
                User user = chatRoomUser.User;
                if(user.Id != userId)
                    users.Add(new UserViewModel()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        UserName = user.UserName
                    });
            }
            
            result.Add(new ChatRoomViewModel()
            {
                Id = chatRoom.Id,
                Name = chatRoom.Name,
                Users = users,
                NewestMessage = messageService.GetNewestMessagesByChatRoomId(chatRoom.Id)
            });
        }
        
        return result;
    }

    public ChatRoomViewModel CreateChatRoom(string fromUserId, string toUserId)
    {
        ChatRoom result;
        ChatRoom? existedChatRoom = chatRoomRepository.GetChatRoomByUsers(fromUserId, toUserId);
        if (existedChatRoom == null)
        {
            List<ChatRoomUser> chatRoomUsers = new List<ChatRoomUser>();
            User? fromUser = userRepository.GetUserById(fromUserId);
            User? toUser = userRepository.GetUserById(toUserId);
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

            result = new ChatRoom()
            {
                Id = Guid.NewGuid().ToString(),
                Users = chatRoomUsers
            };
        
            chatRoomRepository.Add(result);
        }
        else
        {
            result = existedChatRoom;
        }
        
        return new ChatRoomViewModel()
        {
            Id = result.Id,
            Name = result.Name
        };
    }
}