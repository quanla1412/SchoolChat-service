using Microsoft.EntityFrameworkCore;
using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class ChatRoomRepositoryImpl(ChatDbContext context) : IChatRoomRepository
{
    public List<ChatRoom> GetChatRoomsByUserId(string userId)
    {
        List<ChatRoom> result = context.ChatRooms.Where(chatRoom => chatRoom.Users.Any(user => user.User.Id == userId))
            .Include(chatRoom => chatRoom.Users)
            .ThenInclude(charRoomUser => charRoomUser.User)
            .ToList();
        return result;
    }

    public ChatRoom? GetChatRoomByUsers(string fromUserId, string toUserId)
    {
        ChatRoom? result = context.ChatRooms
            .First(chatRoom => 
                chatRoom.Users.Count(user => user.User.Id == fromUserId || user.User.Id == toUserId) == 2
                );

        return result;
    }

    public void Add(ChatRoom chatRoom)
    {
        context.ChatRooms.Add(chatRoom);
        context.SaveChanges();
    }
}