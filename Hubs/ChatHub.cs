using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using SchoolChat.Service.DataService;
using SchoolChat.Service.Models;
using SchoolChat.Service.Service;
using SchoolChat.Service.Service.ServiceImpl;
using SchoolChat.Service.ViewModel;
using Hub = Microsoft.AspNetCore.SignalR.Hub;

namespace SchoolChat.Service.Hubs;

[Authorize]
public class ChatHub(
    SharedDb shared, 
    IMessageService messageService,
    IUserService userService
    ) : Hub
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"User {Context.User?.Identity?.Name} connected.");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (shared.connections.Remove(Context.ConnectionId, out UserConnection conn))
        {
            Clients.AllExcept(Context.ConnectionId)
                .SendAsync("NewUserOfflineListener", conn.UserId);
        }
        return base.OnDisconnectedAsync(exception);
    }

    public async Task ConnectToHub(UserConnection conn)
    {
        shared.connections[Context.ConnectionId] = conn;
        await Clients.AllExcept(Context.ConnectionId)
            .SendAsync("NewUserOnlineListener", conn.UserId);
        
        List<string> onlineUserIds = shared.connections.Values.Select(connection => connection.UserId).ToList();
        await Clients.Client(Context.ConnectionId)
            .SendAsync("OnlineUserIds", onlineUserIds);
    }

    public async Task JoinSpecificChatRoom(string chatRoomId)
    {
        if (shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
        {
            conn.ChatRoomId = chatRoomId;
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoomId);
        
            messageService.MarkReadMessageByChatRoomId(conn.ChatRoomId, conn.UserId);
        
            await Clients.Group(conn.ChatRoomId)
                .SendAsync("JoinSpecificChatRoom", "admin", $"{conn.UserId} has joined {conn.ChatRoomId}");
        }
    }

    public async Task SendMessage(SendMessageModel model)
    {
        if (shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
        {
            CreateMessageViewModel createMessageViewModel = new CreateMessageViewModel()
            {
                ChatRoomId = conn.ChatRoomId,
                FromUserId = conn.UserId,
                Text = model.Message,
                Type = model.Type
            };
            MessageViewModel message = messageService.Add(createMessageViewModel);
            
            await Clients.Group(conn.ChatRoomId)
                .SendAsync("ReceiveMessage", message);
            
            //Broadcast to all user in chatroom to update list chatroom
            List<string> chatRoomUserIds = userService.GetUserIdsByChatRoom(conn.ChatRoomId);
            List<string> connectionIds = new List<string>();
            foreach (var key in shared.connections.Keys)
            {
                if (shared.connections.TryGetValue(key, out UserConnection c) && chatRoomUserIds.Contains(c.UserId) && c.UserId != conn.UserId)
                {
                    connectionIds.Add(key);
                }
            }
            await Clients.Clients(connectionIds)
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
    
    public async Task ForwardMessage(ForwardMessageModel model)
    {
        if (shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
        {
            try
            {
                var forwardedMessage = messageService.ForwardMessage(model, conn.UserId);
                await Clients.Group(model.ChatRoomId)
                    .SendAsync("ReceiveMessage", forwardedMessage);
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync(ex.Message);
            }
        }
    }
}