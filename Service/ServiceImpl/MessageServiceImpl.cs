using Microsoft.AspNetCore.Identity;
using SchoolChat.Service.Models;
using SchoolChat.Service.Repository;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public class MessageServiceImpl(
    IMessageRepository messageRepository, 
    IReadMessageStatusRepository readMessageStatusRepository,
    IDeleteMessageUserRepository deleteMessageUserRepository,
    IUserRepository userRepository
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
            
            User? user = userRepository.GetUserById(message.FromUserId);
                
            result.Add(new MessageViewModel()
            {
                Id = message.Id,
                ChatRoomId = message.ChatRoomId,
                FromUser = new()
                {
                    Id = message.FromUserId,
                    Name = user?.Name ?? user?.Email
                },
                SentDate = message.SentDate,
                Text = !message.IsUnsent ? message.Text : "Tin nhắn đã được thu hồi",
                IsPinned = message.IsPinned,
                IsUnsent = message.IsUnsent,
                IsForwarded = message.IsForwarded,
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
            FromUser = new()
            {
                Id = message.FromUserId
            },
            SentDate = message.SentDate,
            Text = message.Text,
            ReadStatuses = readStatusViewModels
        };
    }

    public PinnedMessageViewModel? GetPinnedMessagesByChatRoomId(string chatRoomId)
    {
        Message? message = messageRepository.GetPinnedMessageByChatRoomId(chatRoomId);
        if (message == null)
            return null;
        
        User? fromUser = userRepository.GetUserById(message.FromUserId);
        
        return new PinnedMessageViewModel()
        {
            Id = message.Id,
            Text = message.Text,
            FromUser = new ShortUserViewModel()
            {
                Id = message.FromUserId,
                Name = fromUser?.Name ?? fromUser?.Email
            }
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
            FromUser = new ShortUserViewModel()
            {
                Id = message.FromUserId
            },
            Text = message.Text,
            SentDate = message.SentDate
        };
    }
    
    public MessageViewModel? ForwardMessage(ForwardMessageModel model, string currentUserId)
    {
        Message? message = messageRepository.GetMessageById(model.MessageId);
        if (message == null)
            return null;

        Message forwardedMessage = new Message
        {
            Id = Guid.NewGuid().ToString(),
            ChatRoomId = model.ChatRoomId,
            FromUserId = currentUserId,
            Text = message.Text,
            SentDate = DateTime.UtcNow,
            ReadStatuses = new List<ReadMessageStatus>(),
            IsForwarded = true,
            IsPinned = false
        };

        messageRepository.Add(forwardedMessage);
        
        return new MessageViewModel()
        {
            Id = message.Id,
            ChatRoomId = model.ChatRoomId,
            FromUser = new ShortUserViewModel()
            {
                Id = message.FromUserId
            },
            Text = message.Text,
            SentDate = message.SentDate,
            IsForwarded = true
        };
    }

    public PinnedMessageViewModel? PinMessage(string messageId)
    {
        Message? message = messageRepository.GetMessageById(messageId);
        if (message == null)
            return null;
        
        Message? pinMessage = messageRepository.GetPinnedMessageByChatRoomId(message.ChatRoomId);
        if (pinMessage != null)
        {
            pinMessage.IsPinned = false;
            messageRepository.Update(pinMessage);
        }
        
        message.IsPinned = true;
        
        messageRepository.Update(message);
        
        User? fromUser = userRepository.GetUserById(message.FromUserId);
        ShortUserViewModel? fromUserViewModel = null;
        if (fromUser != null)
        {
            fromUserViewModel = new ShortUserViewModel()
            {
                Id = message.FromUserId,
                Name = fromUser.Name ?? fromUser.Email
            };
        }
        
        return new PinnedMessageViewModel()
        {
            Id = pinMessage.Id,
            ChatRoomId = message.ChatRoomId,
            FromUser = fromUserViewModel,
            Text = message.Text
        };
    }

    public void UnpinMessage(string messageId)
    {
        Message? message = messageRepository.GetMessageById(messageId);
        if (message == null)
            return;
        message.IsPinned = false;
        
        messageRepository.Update(message);
    }

    public bool UnsentMessage(string messageId)
    {
        Message? message = messageRepository.GetMessageById(messageId);
        
        if (message == null)
            return false;
        
        message.IsUnsent = true;
        
        messageRepository.Update(message);
        
        return true;
    }

    public Boolean DeleteMessage(string messageId, string currentUserId)
    {
        DeleteMessageUser message = new DeleteMessageUser()
        {
            Id = Guid.NewGuid().ToString(),
            MessageId = messageId,
            UserId = currentUserId
        };
        
        deleteMessageUserRepository.Add(message);
        
        return true;
    }
}