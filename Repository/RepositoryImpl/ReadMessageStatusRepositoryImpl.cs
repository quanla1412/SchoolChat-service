using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class ReadMessageStatusRepositoryImpl(ChatDbContext context) : IReadMessageStatusRepository
{
    public ReadMessageStatus? GetByMessageIdAndUserId(string messageId, string userId)
    {
        return context.ReadMessageStatuses
            .FirstOrDefault(ReadMessageStatus => ReadMessageStatus.MessageId == messageId && ReadMessageStatus.UserId == userId);
    }

    public void Add(ReadMessageStatus readMessageStatus)
    {
        context.ReadMessageStatuses.Add(readMessageStatus);
        context.SaveChanges();
    }
}