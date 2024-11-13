using ReadMessageStatus = SchoolChat.Service.Models.ReadMessageStatus;

namespace SchoolChat.Service.Repository;

public interface IReadMessageStatusRepository
{
    ReadMessageStatus? GetByMessageIdAndUserId(string messageId, string userId);
    void Add(ReadMessageStatus readMessageStatus);
}