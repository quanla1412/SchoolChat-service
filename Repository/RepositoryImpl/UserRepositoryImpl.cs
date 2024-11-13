using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class UserRepositoryImpl : IUserRepository
{
    private readonly ChatDbContext _context;

    public UserRepositoryImpl(ChatDbContext context)
    {
        _context = context;
    }

    public User? GetUserById(string id)
    {
        return _context.Users.SingleOrDefault(u => u.Id == id);
    }

    public List<User> GetUsers(string searchString, string? excludeUserId )
    {
        var query = _context.Users.Where(user => user.Email.Contains(searchString));
        if (excludeUserId != null)
        {
            query = query.Where(user => user.Id != excludeUserId);
        }
        return query.ToList();
    }
}