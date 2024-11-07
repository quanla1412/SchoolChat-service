using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolChat.Service;
using SchoolChat.Service.DataService;
using SchoolChat.Service.Hubs;
using SchoolChat.Service.Models;
using SchoolChat.Service.Repository;
using SchoolChat.Service.Repository.RepositoryImpl;
using SchoolChat.Service.Service;
using SchoolChat.Service.Service.ServiceImpl;

var builder = WebApplication.CreateBuilder(args);

// Add service to the container
// Authorization
builder.Services.AddAuthorization();

// Configure identity database access via EF Core.
/*builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseInMemoryDatabase("AppDb"));*/
builder.Services.AddDbContext<ChatDbContext>(
    options => options.UseSqlServer("Server=QUANLEANH;Database=SchoolChat;Trusted_Connection=True;TrustServerCertificate=True"));

// Activate identity APIs. By default, both cookies and proprietary tokens are activated.
// Cookies will be issued based on the `yseCookies` querystring parameter in the login endpoint.
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<ChatDbContext>();

// Add services to the container.
builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("reactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSingleton<SharedDb>();

builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<IChatRoomService, ChatRoomServiceImpl>();
builder.Services.AddScoped<IMessageService, MessageServiceImpl>();

builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();
builder.Services.AddScoped<IChatRoomRepository, ChatRoomRepositoryImpl>();
builder.Services.AddScoped<IMessageRepository, MessageRepositoryImpl>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.MapHub<ChatHub>("/Chat");

app.UseCors("reactApp");

app.MapIdentityApi<User>();

app.Run();

namespace SchoolChat.Service
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
