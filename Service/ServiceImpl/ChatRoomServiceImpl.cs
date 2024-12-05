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
    private string GetChatRoomName(ChatRoom chatRoom, List<UserViewModel> users, string currentUserId)
    {
        if (chatRoom.Name != null)
            return chatRoom.Name;
        
        List<UserViewModel> filteredUsers = users.Where(u => u.Id != currentUserId).ToList();
        return string.Join(", ", filteredUsers.Select(user => user.Name ?? user.Email).ToList());
    }
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
                Name = GetChatRoomName(chatRoom, users, userId),
                Users = users,
                NewestMessage = messageService.GetNewestMessagesByChatRoomId(chatRoom.Id)
            });
        }
        
        return result;
    }

    public ChatRoomDetailViewModel? GetChatRoomDetailById(string id, string currentUserId)
    {
        ChatRoom? chatRoom = chatRoomRepository.GetChatRoomById(id);
        if (chatRoom == null)
        {
            return null;
        }
        
        List<ShortUserViewModel> users = new();
        foreach (var chatRoomUser in chatRoom.Users)
        {
            User user = chatRoomUser.User;
            if(user.Id != currentUserId)
                users.Add(new ShortUserViewModel()
                {
                    Id = user.Id,
                    Name = user.Name ?? user.Email
                });
        }
        
        return new ChatRoomDetailViewModel()
        {
            Id = chatRoom.Id,
            Name = chatRoom.Name,
            PinnedMessage = messageService.GetPinnedMessagesByChatRoomId(chatRoom.Id),
            Users = users
        };
    }

    public ChatRoomViewModel CreateChatRoom(CreateChatRoomViewModel model)
    {
        ChatRoom result;
        ChatRoom? existedChatRoom = chatRoomRepository.GetChatRoomByUsers(model.FromUserId, model.ToUserIds);
        if (existedChatRoom == null)
        {
            List<ChatRoomUser> chatRoomUsers = new List<ChatRoomUser>();
            User? fromUser = userRepository.GetUserById(model.FromUserId);
            if (fromUser == null) 
                throw new Exception("User not found");
        
            chatRoomUsers.Add(new ChatRoomUser()
            {
                Id = Guid.NewGuid().ToString(),
                User = fromUser
            });
            
            model.ToUserIds.ForEach(toUserId =>
            {
                User? toUser = userRepository.GetUserById(toUserId);
                if (toUser == null) 
                    throw new Exception("User not found");
                
                chatRoomUsers.Add(new ChatRoomUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    User = toUser
                });
            });

            result = new ChatRoom()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
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