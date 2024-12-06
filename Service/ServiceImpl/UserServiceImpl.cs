using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
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
            Phone = user.PhoneNumber,
            Avatar = user.Avatar,
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

    public List<string> GetUserIdsByChatRoom(string chatRoomId)
    {
        List<User> users = userRepository.GetUsersByChatRoomId(chatRoomId);
        return users.Select(u => u.Id).ToList();
    }

    public UpdateProfileViewModel UpdateProfile(UpdateProfileViewModel model)
    {
        User user = userRepository.GetUserById(model.Id);
        
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (!model.AvatarFiles.IsNullOrEmpty() && model.AvatarFiles?[0] != null)
        {
            Task<string?> avatarPath = SaveAvatar(model.AvatarFiles[0]);
            user.Avatar = avatarPath.Result;
        }
        
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

    private async Task<string?> SaveAvatar(IFormFile file)
    {
        string baseDirectory = Path.Combine("D:/Study/DotNet/Images");
        if (file.Length > 0) {
            string filePath = Path.Combine(baseDirectory, file.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(fileStream);
            }
        }
        else
        {
            return null;
        }
        return file.FileName;
    }
}