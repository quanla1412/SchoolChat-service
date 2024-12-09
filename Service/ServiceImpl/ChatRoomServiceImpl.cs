using Microsoft.IdentityModel.Tokens;
using SchoolChat.Service.Models;
using SchoolChat.Service.Repository;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public class ChatRoomServiceImpl(
    IUserRepository userRepository, 
    IChatRoomRepository chatRoomRepository,
    IMessageService messageService,
    IUserTaskService userTaskService
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
                Avatar = chatRoom.Avatar,
                IsSingle = users.Count == 1,
                NewestMessage = messageService.GetNewestMessagesByChatRoomId(chatRoom.Id)
            });
        }
        
        result.Sort((a, b) => b.NewestMessage?.SentDate.CompareTo(a.NewestMessage?.SentDate) ?? 0);
        
        return result;
    }

    public ChatRoomDetailViewModel? GetChatRoomDetailById(string id, string currentUserId)
    {
        ChatRoom? chatRoom = chatRoomRepository.GetChatRoomById(id);
        bool isAccepted = false;
        if (chatRoom == null)
        {
            return null;
        }
        
        List<ShortUserViewModel> users = new();
        ShortUserViewModel anotherUser = new();
        foreach (var chatRoomUser in chatRoom.Users)
        {
            User user = chatRoomUser.User;
            ShortUserViewModel shortUserViewModel = new ShortUserViewModel()
            {
                Id = user.Id,
                Name = user.Name ?? user.Email,
                Avatar = user.Avatar,
            };
            users.Add(shortUserViewModel);
            if (currentUserId != user.Id)
            {
                isAccepted = chatRoomUser.IsAccepted;
                anotherUser = shortUserViewModel;
            }
        }
        
        List<UserTaskViewModel> userTasks = userTaskService.GetByChatRoomId(id);
        
        bool isSingle = users.Count == 2;
        
        
        return new ChatRoomDetailViewModel()
        {
            Id = chatRoom.Id,
            Name = !isSingle ? chatRoom.Name : anotherUser.Name,
            Avatar = !isSingle ? chatRoom.Avatar : anotherUser.Avatar,
            PinnedMessage = messageService.GetPinnedMessagesByChatRoomId(chatRoom.Id),
            Users = users,
            IsAccepted = isAccepted,
            IsSingle = isSingle,
            Tasks = userTasks
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
                User = fromUser,
                IsAccepted = true
            });
            
            model.ToUserIds.ForEach(toUserId =>
            {
                User? toUser = userRepository.GetUserById(toUserId);
                if (toUser == null) 
                    throw new Exception("User not found");
                
                chatRoomUsers.Add(new ChatRoomUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    User = toUser,
                    IsAccepted = false
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

    public ChatRoomViewModel Update(UpdateChatRoomVIewModel model)
    {
        ChatRoom? chatRoom = chatRoomRepository.GetChatRoomById(model.Id);
        if (chatRoom == null)
            throw new Exception("Chat room not found");
        
        chatRoom.Name = model.Name ?? chatRoom.Name;
        if (!model.Avatar.IsNullOrEmpty() && model.Avatar[0].Length > 0)
        {
            chatRoom.Avatar = SaveAvatar(model.Avatar[0]).Result;
        }
        
        chatRoomRepository.Update(chatRoom);
        return new ChatRoomViewModel()
        {
            Id = chatRoom.Id,
            Name = chatRoom.Name,
            Avatar = chatRoom.Avatar,
        };
    }

    public List<ShortUserViewModel> AddUserToChatRoom(AddUserToChatRoomModel model)
    {
        List<ChatRoomUser> chatRoomUsers = new();
        List<ShortUserViewModel> result = new();
        model.UserIds.ForEach(userId =>
        {
            User? user = userRepository.GetUserById(userId);
            if(user == null)
                throw new Exception("User not found");
            chatRoomUsers.Add(new ChatRoomUser()
            {
                Id = Guid.NewGuid().ToString(),
                ChatRoomId = model.ChatRoomId,
                IsAccepted = false,
                User = user
            });
            result.Add(new ShortUserViewModel() { Id = user.Id, Name = user.Name });
        });
        
        chatRoomRepository.AddChatRoomUser(chatRoomUsers);
        return result;
    }

    private async Task<string?> SaveAvatar(IFormFile file)
    {
        string baseDirectory = Path.Combine("D:/Study/DotNet/Images");
        if (file.Length > 0) {
            string filePath = Path.Combine(baseDirectory, file.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(fileStream);
            }
        }
        else
        {
            return null;
        }
        return file.FileName;
    }
}