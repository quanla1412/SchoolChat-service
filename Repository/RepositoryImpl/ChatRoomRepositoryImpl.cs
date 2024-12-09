using Microsoft.EntityFrameworkCore;
using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class ChatRoomRepositoryImpl(ChatDbContext context) : IChatRoomRepository
{
    public ChatRoom? GetChatRoomById(string id)
    {
        return context.ChatRooms.Include(chatRoom => chatRoom.Users)
            .ThenInclude(charRoomUser => charRoomUser.User)
            .FirstOrDefault(room => room.Id == id);
    }

    public List<ChatRoom> GetChatRoomsByUserId(string userId)
    {
        List<ChatRoom> result = context.ChatRooms.Where(chatRoom => chatRoom.Users.Any(user => user.User.Id == userId))
            .Include(chatRoom => chatRoom.Users)
            .ThenInclude(charRoomUser => charRoomUser.User)
            .ToList();
        return result;
    }

    public ChatRoom? GetChatRoomByUsers(string fromUserId, List<string> toUserIds)
    {
        ChatRoom? result = context.ChatRooms
            .FirstOrDefault(chatRoom => 
                chatRoom.Users.Count(user => user.User.Id == fromUserId || toUserIds.Contains(user.User.Id)) == toUserIds.Count + 1
                );

        return result;
    }

    public void Add(ChatRoom chatRoom)
    {
        context.ChatRooms.Add(chatRoom);
        context.SaveChanges();
    }

    public void Update(ChatRoom chatRoom)
    {
        context.ChatRooms.Update(chatRoom);
        context.SaveChanges();
    }

    public void AddChatRoomUser(List<ChatRoomUser> chatRoomUsers)
    {
        context.ChatRoomUsers.AddRange(chatRoomUsers);
        context.SaveChanges();
    }
}