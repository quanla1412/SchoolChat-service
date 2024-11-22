using SchoolChat.Service.Models;
using SchoolChat.Service.Repository;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public class MessageServiceImpl(
    IMessageRepository messageRepository, 
    IReadMessageStatusRepository readMessageStatusRepository
    ) : IMessageService
{
    public List<MessageViewModel> GetMessagesByChatRoomId(string chatRoomId)
    {
        List<Message> messages = messageRepository.GetMessagesByChatRoomId(chatRoomId);
        List<MessageViewModel> result = new List<MessageViewModel>();
        
        messages.ForEach(message =>
        {
            List<ReadMessageStatusViewModel> readStatusViewModels = new List<ReadMessageStatusViewModel>();
            if(message.ReadStatuses != null)
                message.ReadStatuses.ForEach(readStatus => readStatusViewModels.Add(new ReadMessageStatusViewModel()
                {
                    Id = readStatus.Id,
                    MessageId = readStatus.Id,
                    UserId = readStatus.UserId,
                    ReadDate = readStatus.ReadDate
                }));
                
            result.Add(new MessageViewModel()
            {
                Id = message.Id,
                ChatRoomId = message.ChatRoomId,
                FromUserId = message.FromUserId,
                SentDate = message.SentDate,
                Text = message.Text,
                ReadStatuses = readStatusViewModels
            });
        });

        return result;
    }

    public MessageViewModel? GetNewestMessagesByChatRoomId(string chatRoomId)
    {
        Message? message = messageRepository.GetNewestMessagesByChatRoomId(chatRoomId);
        if (message == null)
            return null;
        
        List<ReadMessageStatusViewModel> readStatusViewModels = new List<ReadMessageStatusViewModel>();
        message.ReadStatuses.ForEach(readStatus => readStatusViewModels.Add(new ReadMessageStatusViewModel()
        {
            Id = readStatus.Id,
            MessageId = readStatus.Id,
            UserId = readStatus.UserId,
            ReadDate = readStatus.ReadDate
        }));

        return new MessageViewModel()
        {
            Id = message.Id,
            ChatRoomId = message.ChatRoomId,
            FromUserId = message.FromUserId,
            SentDate = message.SentDate,
            Text = message.Text,
            ReadStatuses = readStatusViewModels
        };
    }

    public ReadMessageStatusViewModel MarkReadMessage(string messageId, string userId)
    {
        ReadMessageStatus readMessageStatus = new ReadMessageStatus()
        {
            Id = Guid.NewGuid().ToString(),
            MessageId = messageId,
            ReadDate = DateTime.Now,
            UserId = userId
        };
        
        readMessageStatusRepository.Add(readMessageStatus);

        return new ReadMessageStatusViewModel()
        {
            Id = readMessageStatus.Id,
            MessageId = readMessageStatus.MessageId,
            ReadDate = readMessageStatus.ReadDate,
            UserId = readMessageStatus.UserId
        };
    }

    public void MarkReadMessageByChatRoomId(string chatRoomId, string userId)
    {
        List<Message> messages = messageRepository.GetMessagesByChatRoomId(chatRoomId);
        messages = messages.Where(message => message.FromUserId != userId).ToList();
        
        messages.ForEach(message => {
            ReadMessageStatus? readMessageStatus = readMessageStatusRepository.GetByMessageIdAndUserId(message.Id, userId);
            if(readMessageStatus == null)
                readMessageStatusRepository.Add(new ReadMessageStatus()
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageId = message.Id,
                    ReadDate = DateTime.Now,
                    UserId = userId
                });
        });
    }

    public MessageViewModel Add(CreateMessageViewModel createModel)
    {
        Message message = new Message()
        {
            Id = Guid.NewGuid().ToString(),
            ChatRoomId = createModel.ChatRoomId,
            FromUserId = createModel.FromUserId,
            Text = createModel.Text,
            SentDate = DateTime.Now
        };
        
        messageRepository.Add(message);

        return new MessageViewModel()
        {
            Id = message.Id,
            ChatRoomId = createModel.ChatRoomId,
            FromUserId = message.FromUserId,
            Text = message.Text,
            SentDate = message.SentDate
        };
    }
    
    public Message ForwardMessage(ForwardMessageModel model)
    {
        Message message = messageRepository.GetMessageById(model.MessageId);
        if (message == null)
            return null;

        Message forwardedMessage = new Message
        {
            Id = Guid.NewGuid().ToString(),
            ChatRoomId = model.ChatRoomId,
            FromUserId = message.FromUserId,
            Text = message.Text,
            SentDate = DateTime.UtcNow,
            ReadStatuses = new List<ReadMessageStatus>(),
            IsForwarded = true,
            IsPinned = false
        };

        messageRepository.Add(forwardedMessage);
        return forwardedMessage;
    }

    public Message PinMessage(string messageId)
    {
        Message message = messageRepository.GetMessageById(messageId);
        if (message == null)
            return null;

        message.IsPinned = true;
        
        messageRepository.Update(message);
        return message;
    }
}