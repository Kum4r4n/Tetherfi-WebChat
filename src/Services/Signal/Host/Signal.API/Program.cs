using Common.Authentication;
using Signal.Application.Hubs;
using Signal.Application.Interfaces.Repository;
using Signal.Application.Models;
using Signal.Infrastructure.Proto;
using Signal.Infrastructure.Providers;
using Signal.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSignalR();

//services
builder.Services.AddScoped<UserDataGrpcProvider>();
builder.Services.AddScoped<SignalHub>();
builder.Services.AddGrpcClient<UserDataService.UserDataServiceClient>(opt => opt.Address = new Uri(builder.Configuration.GetValue<string>("URLS:Message") ?? string.Empty));

//repositories
builder.Services.AddScoped<IUserDataRepository, UserDataRepository>();


builder.Services.AddCors();
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
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuth();


app.MapControllers();
app.MapHub<SignalHub>("/signalhub");
app.Run();
