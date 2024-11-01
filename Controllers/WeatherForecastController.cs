using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolChat.Service.Models;

namespace SchoolChat.Service.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    
    private readonly ChatDbContext _dbContext;

    private readonly UserManager<User> _userManager;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ChatDbContext dbContext, UserManager<User> userManager)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [ActionName("GetWeatherForecast")]
    public IEnumerable<ChatRoom> Get()
    {
        _logger.Log(LogLevel.Information, "User {IdentityUser} logged in", _userManager.GetUserId(User) );
        /*return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();*/

        return _dbContext.ChatRooms.ToArray();
    }
}
