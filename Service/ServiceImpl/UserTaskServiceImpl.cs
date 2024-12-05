using Microsoft.AspNetCore.Http.HttpResults;
using SchoolChat.Service.Models;
using SchoolChat.Service.Repository;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public class UserTaskServiceImpl(
    IUserTaskRepository userTaskRepository,
    ITaskAssigneeRepository taskAssigneeRepository,
    IUserRepository userRepository
    ) : IUserTaskService
{
    public List<UserTaskViewModel> GetByChatRoomId(string chatRoomId)
    {  
        List<UserTask> tasks = userTaskRepository.GetByChatRoomId(chatRoomId);
        List<UserTaskViewModel> taskViewModels = new List<UserTaskViewModel>();
        
        foreach (var task in tasks)
        {
            List<TaskAssignee> taskAssignees = taskAssigneeRepository.GetByTaskId(task.Id);
            List<UserViewModel> assigneeViewModels = taskAssignees
                .Select(ta => 
                {
                    var user = userRepository.GetUserById(ta.UserId);
                    return new UserViewModel
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email
                    };
                })
                .ToList();
            
            var taskViewModel = new UserTaskViewModel
            {
                Id = task.Id,
                ChatRoomId = task.ChatRoomId,
                CreatorId = task.CreatorId,
                Message = task.Message,
                Deadline = task.Deadline,
                CreatedAt = task.CreatedAt,
                Status = task.Status,
                TaskAssignees = assigneeViewModels
            };

            taskViewModels.Add(taskViewModel);
        }
        
        return taskViewModels;
    }
    
    public UserTaskViewModel GetById(string id)
    {
        var task = userTaskRepository.GetById(id);
        if (task == null) 
            throw new KeyNotFoundException("Task not found.");

        List<TaskAssignee> taskAssignees = taskAssigneeRepository.GetByTaskId(task.Id);
        List<UserViewModel> assigneeViewModels = taskAssignees
            .Select(ta => 
            {
                var user = userRepository.GetUserById(ta.UserId);
                return new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };
            })
            .ToList();

        return new UserTaskViewModel
        {
            Id = task.Id,
            ChatRoomId = task.ChatRoomId,
            CreatorId = task.CreatorId,
            Message = task.Message,
            Deadline = task.Deadline,
            CreatedAt = task.CreatedAt,
            Status = task.Status,
            TaskAssignees = assigneeViewModels
        };
    }

    public UserTaskViewModel Create(CreateTaskViewModel task)
    {
        UserTask userTask = new UserTask()
        {
            Id = Guid.NewGuid().ToString(),
            ChatRoomId = task.ChatRoomId,
            CreatorId = task.CreatorId,
            Message = task.Message,
            Deadline = task.Deadline,
            CreatedAt = DateTime.Now,
            Status = task.Status
        };
    
        userTaskRepository.Add(userTask);
        
        foreach (var assigneeId in task.TaskAssignees)
        {
            var taskAssignee = new TaskAssignee
            {
                Id = Guid.NewGuid().ToString(),
                TaskId = userTask.Id,
                UserId = assigneeId
            };
            taskAssigneeRepository.Add(taskAssignee);
        }

        return new UserTaskViewModel()
        {
            Id = userTask.Id,
            ChatRoomId = userTask.ChatRoomId,
            CreatorId = userTask.CreatorId,
            Message = userTask.Message,
            Deadline = userTask.Deadline,
            CreatedAt = userTask.CreatedAt,
            Status = userTask.Status,
            TaskAssignees = task.TaskAssignees?.Select(id => new UserViewModel { Id = id }).ToList()
        };
    }
    
    public UserTaskViewModel Update(UserTaskViewModel task)
    {
        UserTask? existingTask = userTaskRepository.GetById(task.Id);
        if (existingTask == null) 
            throw new KeyNotFoundException("Task not found.");

        existingTask.Message = task.Message;
        existingTask.Deadline = task.Deadline;
        existingTask.Status = task.Status;

        userTaskRepository.Update(existingTask);
        taskAssigneeRepository.DeleteByTaskId(existingTask.Id);

        foreach (UserViewModel assignee in task.TaskAssignees)
        {
            var taskAssignee = new TaskAssignee
            {
                Id = Guid.NewGuid().ToString(),
                TaskId = existingTask.Id,
                UserId = assignee.Id
            };
            taskAssigneeRepository.Add(taskAssignee);
        }

        return new UserTaskViewModel
        {
            Id = existingTask.Id,
            ChatRoomId = existingTask.ChatRoomId,
            CreatorId = existingTask.CreatorId,
            Message = existingTask.Message,
            Deadline = existingTask.Deadline,
            CreatedAt = existingTask.CreatedAt,
            Status = existingTask.Status,
            TaskAssignees = task.TaskAssignees?.Select(user => new UserViewModel { Id = user.Id }).ToList()
        };
    }
    
    public Boolean Delete(string id)
    {
        UserTask? existingTask = userTaskRepository.GetById(id);
        if (existingTask == null)
            return false;

        taskAssigneeRepository.DeleteByTaskId(existingTask.Id);
    
        userTaskRepository.Delete(id);
    
        return true;
    }
}