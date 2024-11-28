using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class DeleteMessageUserRepositoryImpl(ChatDbContext context) : IDeleteMessageUserRepository
{
    public void Add(DeleteMessageUser message)
    {
        context.DeleteMessageUsers.Add(message);
        context.SaveChanges();
    }
}