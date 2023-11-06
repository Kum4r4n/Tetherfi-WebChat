using Common.Authentication;
using Identity.Application.Interfaces.Repositories;
using Identity.Application.Interfaces.Services;
using Identity.Application.Services;
using Identity.Infrastructure.Configuration;
using Identity.Infrastructure.Context;
using Identity.Infrastructure.Providers;
using Identity.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//db
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("SQL")));


//services
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddGrpc();


//repos
builder.Services.AddScoped<IUserRepository, UserRepository>();

//settings
var tokenSetting = builder.Configuration.GetSection("TokenSetting").Get<TokenSetting>();
builder.Services.AddSingleton(tokenSetting);
builder.Services.AddScoped<Identity.Application.Interfaces.IConfigurationProvider, Identity.Infrastructure.Configuration.ConfigurationProvider>();
builder.Services.AddAuth(tokenSetting.Secret);

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGrpcService<UserGrpcProvider>();
app.UseHttpsRedirection();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuth();

app.MapControllers();

app.Run();
