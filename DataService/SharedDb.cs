using System.Collections.Concurrent;
using ChatAllNight.ChatService.Models;

namespace ChatAllNight.ChatService.DataService;

public class SharedDb
{
    private readonly ConcurrentDictionary<string, UserConnection> _connections = new();
    
    public ConcurrentDictionary<string, UserConnection> connections => _connections;
}