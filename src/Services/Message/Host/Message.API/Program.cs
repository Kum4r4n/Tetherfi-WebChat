using Common.Authentication;
using Message.Application.Hubs;
using Message.Application.Interfaces;
using Message.Application.Interfaces.Repositories;
using Message.Application.Services;
using Message.Infrastructure.Context;
using Message.Infrastructure.Providers;
using Message.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

//db
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("SQL")));

//services
builder.Services.AddScoped<UserGrpcProvider>();
builder.Services.AddScoped<IUserService, Message.Application.Services.UserService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<UserChatHub>();
builder.Services.AddGrpc();
builder.Services.AddGrpcClient<Message.Infrastructure.Proto.UserService.UserServiceClient>(opt => opt.Address = new Uri(builder.Configuration.GetValue<string>("URLS:Identity")));


//repositories
builder.Services.AddScoped<IUserConnectionInfoRepository, UserConnectionInfoRepository>();
builder.Services.AddScoped<IChatsRepository, ChatsRepository>();

builder.Services.AddCors();
//settings
builder.Services.AddAuth("Tetherfi-aADf3GDsMEuVjphnL5c6moW7OM8biQgp99JXeGgp");

builder.WebHost.ConfigureKestrel(option =>
{
    option.Listen(IPAddress.Any, 80, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });

    option.Listen(IPAddress.Any, 9633, listenOptions => {

        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
db.Database.Migrate();


app.MapGrpcService<UserDataGrpcProvider>();

app.UseHttpsRedirection();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuth();

app.MapControllers();

app.MapHub<UserChatHub>("/userchathub");

app.Run();
