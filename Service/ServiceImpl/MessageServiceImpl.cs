using SchoolChat.Service.Models;
using SchoolChat.Service.Repository;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public class MessageServiceImpl(IMessageRepository messageRepository) : IMessageService
{
    public List<MessageViewModel> GetMessagesByChatRoomId(string chatRoomId)
    {
        List<Message> messages = messageRepository.GetMessagesByChatRoomId(chatRoomId);
        List<MessageViewModel> result = new List<MessageViewModel>();
        
        messages.ForEach(message => result.Add(new MessageViewModel()
        {
            Id = message.Id,
            ChatRoomId = message.ChatRoomId,
            FromUserId = message.FromUserId,
            SentDate = message.SentDate,
            Text = message.Text
        }));

        return result;
    }

    public MessageViewModel? GetNewestMessagesByChatRoomId(string chatRoomId)
    {
        Message? message = messageRepository.GetNewestMessagesByChatRoomId(chatRoomId);
        if (message == null)
            return null;

        return new MessageViewModel()
        {
            Id = message.Id,
            ChatRoomId = message.ChatRoomId,
            FromUserId = message.FromUserId,
            SentDate = message.SentDate,
            Text = message.Text
        };
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
}