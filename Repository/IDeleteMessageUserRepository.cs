using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository;

public interface IDeleteMessageUserRepository
{
    void Add(DeleteMessageUser deleteMessageUser);
}