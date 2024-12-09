using SchoolChat.Service.Models;
using SchoolChat.Service.Repository;
using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service.ServiceImpl;

public class UserTaskServiceImpl(
    IUserTaskRepository userTaskRepository,
    ITaskAssigneeRepository taskAssigneeRepository,
    IUserRepository userRepository,
    IUserService userService
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
                        Name = user.Name ?? user.Email,
                        Email = user.Email
                    };
                })
                .ToList();
            
            ShortUserViewModel? creator = userService.GetShortUserById(task.CreatorId);
            
            var taskViewModel = new UserTaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                ChatRoomId = task.ChatRoomId,
                CreatorId = task.CreatorId,
                Description = task.Description,
                Deadline = task.Deadline,
                CreatedDate = task.CreatedDate,
                IsCompleted = task.IsCompleted,
                Creator = creator,
                TaskAssignees = assigneeViewModels
            };

            taskViewModels.Add(taskViewModel);
        }
        taskViewModels.Sort((x, y) => x.Deadline.CompareTo(y.Deadline));
        
        return taskViewModels;
    }

    public List<UserTaskViewModel> GetByUserId(string userId)
    {
        List<UserTask> createdByMeTasks = userTaskRepository.GetByCreatorId(userId);
        List<UserTask> assignToMeTasks = userTaskRepository.GetByAssignToUserId(userId);
        
        List<UserTaskViewModel> taskViewModels = new List<UserTaskViewModel>();
        createdByMeTasks.ForEach(task => taskViewModels.Add(new UserTaskViewModel()
        {
            Id = task.Id,
            Name = task.Name,
            ChatRoomId = task.ChatRoomId,
            CreatorId = task.CreatorId,
            CreatedDate = task.CreatedDate,
            Deadline = task.Deadline,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            Creator = userService.GetShortUserById(task.CreatorId),
            Type = "MINE"
        }));
        
        assignToMeTasks.ForEach(task => taskViewModels.Add(new UserTaskViewModel()
        {
            Id = task.Id,
            Name = task.Name,
            ChatRoomId = task.ChatRoomId,
            CreatorId = task.CreatorId,
            CreatedDate = task.CreatedDate,
            Deadline = task.Deadline,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            Creator = userService.GetShortUserById(task.CreatorId),
            Type = "ASSIGNED_BY_ME"
        }));
        
        taskViewModels.Sort((x, y) => x.Deadline.CompareTo(y.Deadline));
        
        taskViewModels.ForEach(task =>
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
            
            task.TaskAssignees = assigneeViewModels;
        });

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
            Deadline = task.Deadline,
            TaskAssignees = assigneeViewModels
        };
    }

    public UserTaskViewModel Create(CreateTaskViewModel task, string currentUserId)
    {
        UserTask userTask = new UserTask()
        {
            Id = Guid.NewGuid().ToString(),
            Name = task.Name,
            ChatRoomId = task.ChatRoomId,
            Description = task.Description,
            Deadline = task.Deadline,
            IsCompleted = false,
            CreatorId = currentUserId,
            CreatedDate = DateTime.Now,
        };
    
        userTaskRepository.Add(userTask);
        
        foreach (var assigneeId in task.TaskAssigneeIds)
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
            Deadline = userTask.Deadline,
            TaskAssignees = task.TaskAssigneeIds?.Select(id => new UserViewModel { Id = id }).ToList()
        };
    }
    
    public UserTaskViewModel Update(UserTaskViewModel task)
    {
        UserTask? existingTask = userTaskRepository.GetById(task.Id);
        if (existingTask == null) 
            throw new KeyNotFoundException("Task not found.");

        /*existingTask.Message = task.Message;*/
        existingTask.Deadline = task.Deadline;
        /*existingTask.Status = task.Status;*/

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
            Deadline = existingTask.Deadline,
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

    public void CompleteTask(string taskId)
    {
        UserTask? task = userTaskRepository.GetById(taskId);
        if(task == null)
            throw new KeyNotFoundException("Task not found.");
        
        task.IsCompleted = true;
        userTaskRepository.Update(task);
    }
}