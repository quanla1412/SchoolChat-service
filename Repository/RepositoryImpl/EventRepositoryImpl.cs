using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class EventRepositoryImpl(ChatDbContext context) : IEventRepository
{
    public List<Event> GetEventsByUserId(string userId)
    {
        return context.Events.Where(e => e.Users.FirstOrDefault(u => u.User.Id == userId) != null).ToList();
    }

    public void Create(Event newEvent)
    {
        context.Events.Add(newEvent);
        context.SaveChanges();
    }
}