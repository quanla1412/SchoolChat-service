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

    public List<User> GetUsers(string searchString)
    {
        return _context.Users.Where(user => user.Email.Contains(searchString)).ToList();
    }

    public User UpdateProfile(User user)
    {
        var existingUser = _context.Users.Find(user.Id);
        if (existingUser == null)
            throw new KeyNotFoundException("User not found.");

        existingUser.Name = user.Name;
        existingUser.Birthday = user.Birthday;
        existingUser.Gender = user.Gender;
        existingUser.PhoneNumber = user.PhoneNumber;
        
        _context.Users.Update(existingUser);
        _context.SaveChanges();

        return existingUser;
    }
}