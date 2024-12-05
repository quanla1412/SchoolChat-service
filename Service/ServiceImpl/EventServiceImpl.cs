using SchoolChat.Service.Models;
using SchoolChat.Service.Repository;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public class EventServiceImpl(
    IEventRepository eventRepository, 
    IUserRepository userRepository
    ) : IEventService
{
    public List<ShortEventViewModel> GetShortEvents(string currentUserId)
    {
        List<Event> events = eventRepository.GetEventsByUserId(currentUserId);
        List<ShortEventViewModel> result = new List<ShortEventViewModel>();

        foreach (Event e in events)
        {
            result.Add(new ShortEventViewModel()
            {
                Id = e.Id,
                Title = e.Name,
                Start = e.StartDate,
                End = e.EndDate
            });
        }

        return result;
    }

    public EventDetailViewModel Create(CreateEventViewModel model, string currentUserId)
    {
        List<EventUser> eventUsers = new List<EventUser>();
        if (model.ChatRoomId == null)
        {
            eventUsers.Add(new EventUser()
            {
                Id = Guid.NewGuid().ToString(),
                User = userRepository.GetUserById(currentUserId)
            });
        }
        else
        {
            List<User> users = userRepository.GetUsersByChatRoomId(model.ChatRoomId);
            foreach (User user in users)
            {
                eventUsers.Add(new EventUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    User = user
                });
            }
        }
        
        Event newEvent = new Event()
        {
            Id = Guid.NewGuid().ToString(),
            Name = model.Name,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Description = model.Description,
            ChatRoomId = model.ChatRoomId,
            Users = eventUsers
        };
        
        eventRepository.Create(newEvent);

        return new EventDetailViewModel()
        {
            Id = newEvent.Id,
            Name = model.Name,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Description = model.Description,
            ChatRoomId = model.ChatRoomId,
        };
    }
}