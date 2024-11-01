using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class ChatRoomRepositoryImpl : IChatRoomRepository
{
    private readonly ChatDbContext _context;

    public ChatRoomRepositoryImpl(ChatDbContext context)
    {
        _context = context;
    }

    public void Add(ChatRoom chatRoom)
    {
        _context.ChatRooms.Add(chatRoom);
        _context.SaveChanges();
    }
}