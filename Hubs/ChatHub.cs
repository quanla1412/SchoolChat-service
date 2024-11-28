using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using SchoolChat.Service.DataService;
using SchoolChat.Service.Models;
using SchoolChat.Service.Service.ServiceImpl;
using SchoolChat.Service.ViewModel;
using Hub = Microsoft.AspNetCore.SignalR.Hub;

namespace SchoolChat.Service.Hubs;

[Authorize]
public class ChatHub(SharedDb shared, IMessageService messageService) : Hub
{
    public async Task JoinChat(UserConnection conn)
    {
        await Clients.All
            .SendAsync("ReceiveMessage", "admin", $"{conn.Username} has joined!");
    }

    public async Task JoinSpecificChatRoom(UserConnection conn)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoomId);
        
        shared.connections[Context.ConnectionId] = conn;
        
        messageService.MarkReadMessageByChatRoomId(conn.ChatRoomId, conn.UserId);
        
        await Clients.Group(conn.ChatRoomId)
            .SendAsync("JoinSpecificChatRoom", "admin", $"{conn.UserId} has joined {conn.ChatRoomId}");
    }

    public async Task SendMessage(string msg)
    {
        if (shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
        {
            CreateMessageViewModel createMessageViewModel = new CreateMessageViewModel()
            {
                ChatRoomId = conn.ChatRoomId,
                FromUserId = conn.UserId,
                Text = msg
            };
            MessageViewModel message = messageService.Add(createMessageViewModel);
            
            await Clients.Group(conn.ChatRoomId)
                .SendAsync("ReceiveMessage", message);
        }
    }
    
    public async Task PinMessage(string messageId)
    {
        if (shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
        {
            var pinnedMessage = messageService.PinMessage(messageId);
            
            await Clients.Group(conn.ChatRoomId)
                .SendAsync("NewPinnedMessage", pinnedMessage);
        }
    }
    
    public async Task UnpinMessage(string messageId)
    {
        if (shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
        {
            messageService.UnpinMessage(messageId);
            
            await Clients.Group(conn.ChatRoomId)
                .SendAsync("UnpinMessage");
        }
    }
    
    public async Task UnsentMessage(string messageId)
    {
        if (shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
        {
            messageService.UnsentMessage(messageId);
            
            await Clients.Group(conn.ChatRoomId)
                .SendAsync("UnsentMessage", messageId);
        }
    }
    
    public async Task DeleteMessage(string messageId)
    {
        if (shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
        {
            bool isDeleted = messageService.DeleteMessage(messageId, conn.UserId);
            if (isDeleted)
                await Clients.Group(conn.ChatRoomId)
                    .SendAsync("DeleteMessage", messageId);
        }
    }
}