using Common.Authentication;
using Message.Application.Hubs;
using Message.Application.Interfaces.Repositories;
using Message.Infrastructure.Context;
using Message.Infrastructure.Proto;
using Message.Infrastructure.Providers;
using Message.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddGrpcClient<UserService.UserServiceClient>(opt => opt.Address = new Uri("https://localhost:44370"));


//repositories
builder.Services.AddScoped<IUserConnectionInfoRepository, UserConnectionInfoRepository>();


//settings
builder.Services.AddAuth("Tetherfi-aADf3GDsMEuVjphnL5c6moW7OM8biQgp99JXeGgp");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuth();

app.MapControllers();

app.MapHub<UserChatHub>("/userchathub");

app.Run();
