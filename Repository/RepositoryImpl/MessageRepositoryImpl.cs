using Microsoft.EntityFrameworkCore;
using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class MessageRepositoryImpl(ChatDbContext context) : IMessageRepository
{
    public List<Message> GetMessagesByChatRoomId(string chatRoomId)
    {
        return context.Messages
            .Include(message => message.ReadStatuses)
            .Where(message => message.ChatRoomId == chatRoomId)
            .OrderBy(message => message.SentDate)
            .ToList();
    }
    
    public Message? GetNewestMessagesByChatRoomId(string chatRoomId)
    {
        return context.Messages
            .Include(message => message.ReadStatuses)
            .Where(message => message.ChatRoomId == chatRoomId)
            .OrderByDescending(message => message.SentDate)
            .Take(1)
            .FirstOrDefault();
    }

    public void Add(Message message)
    {
        context.Messages.Add(message);
        context.SaveChanges();
    }

    public Message? GetMessageById(string messageId)
    {
        return context.Messages
            .Include(message => message.ReadStatuses)
            .FirstOrDefault(message => message.Id == messageId);
    }
    
    public void Update(Message message)
    {
        context.Messages.Update(message);
        context.SaveChanges();
    }

    public Message? GetPinMessageByChatRoomId(string chatRoomId)
    {
        return context.Messages
            .Include(message => message.ReadStatuses)
            .FirstOrDefault(message => message.ChatRoomId == chatRoomId && message.IsPinned == true);
    }
}