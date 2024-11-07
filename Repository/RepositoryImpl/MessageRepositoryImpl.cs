using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class MessageRepositoryImpl(ChatDbContext context) : IMessageRepository
{
    public List<Message> GetMessagesByChatRoomId(string chatRoomId)
    {
        return context.Messages
            .Where(message => message.ChatRoomId == chatRoomId)
            .OrderBy(message => message.SentDate)
            .ToList();
    }
    
    public Message? GetNewestMessagesByChatRoomId(string chatRoomId)
    {
        return context.Messages
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
}