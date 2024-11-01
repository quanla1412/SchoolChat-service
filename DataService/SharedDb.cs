﻿using System.Collections.Concurrent;
using SchoolChat.Service.Models;

namespace SchoolChat.Service.DataService;

public class SharedDb
{
    private readonly ConcurrentDictionary<string, UserConnection> _connections = new();
    
    public ConcurrentDictionary<string, UserConnection> connections => _connections;
}