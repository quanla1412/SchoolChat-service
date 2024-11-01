using SchoolChat.Service.Models;
using SchoolChat.Service.Repository;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public class UserServiceImpl(IUserRepository userRepository) : IUserService
{
    public UserViewModel? GetUserById(string id)
    {
        User? user = userRepository.GetUserById(id);
        
        if(user == null)
            throw new ArgumentException($"User with id: {id} was not found");

        return new UserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Username = user.UserName,
        };
    }

    public List<UserViewModel> GetUsers(string searchString)
    {
        List<User> users = userRepository.GetUsers(searchString);
        List<UserViewModel> usersViewModel = new List<UserViewModel>();
        
        foreach (User user in users)
        {
            usersViewModel.Add(new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Username = user.UserName,
            });
        }

        return usersViewModel;
    }
}