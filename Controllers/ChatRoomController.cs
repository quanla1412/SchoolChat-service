﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolChat.Service.Models;
using SchoolChat.Service.Service;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ChatRoomController(UserManager<User> userManager, IChatRoomService chatRoomService) : ControllerBase
{
    [HttpPost]
    [ActionName("Create")]
    public IActionResult Create([FromBody] CreateChatRoomViewModel model)
    {
        string currentUserId = userManager.GetUserId(User);
        ChatRoomViewModel result = chatRoomService.CreateChatRoom(currentUserId, model.ToUserId);
        
        return Ok(result);
    }
}