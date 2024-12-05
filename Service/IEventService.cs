using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service;

public interface IEventService
{
    List<ShortEventViewModel> GetShortEvents(string currentUserId);
    EventDetailViewModel Create(CreateEventViewModel model, string currentUserId);
}