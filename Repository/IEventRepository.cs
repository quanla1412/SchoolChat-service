using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository;

public interface IEventRepository
{
    List<Event> GetEventsByUserId(string userId);
    void Create(Event newEvent);
}