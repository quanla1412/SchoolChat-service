using Microsoft.AspNetCore.Http.HttpResults;
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
            Name = user.Name ?? user.Email,
            UserName = user.UserName,
            Gender = user.Gender,
            Birthday = user.Birthday,
            Phone = user.PhoneNumber
        };
    }

    public List<UserViewModel> GetUsers(string searchString, string? excludeUserId)
    {
        List<User> users = userRepository.GetUsers(searchString, excludeUserId);
        List<UserViewModel> usersViewModel = new List<UserViewModel>();
        
        foreach (User user in users)
        {
            usersViewModel.Add(new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                UserName = user.UserName,
            });
        }

        return usersViewModel;
    }

    public UpdateProfileViewModel UpdateProfile(UpdateProfileViewModel model)
    {
        User user = userRepository.GetUserById(model.Id);
        
        if (user == null)
            throw new KeyNotFoundException("User not found.");
        
        user.Name = model.Name;
        user.Birthday = model.Birthday;
        user.Gender = model.Gender;
        user.PhoneNumber = model.Phone;
        
        userRepository.UpdateProfile(user);

        return new UpdateProfileViewModel()
        {
            Id = user.Id,
            Name = user.Name,
            Birthday = user.Birthday,
            Gender = user.Gender,
            Phone = user.PhoneNumber
        };
    }
}