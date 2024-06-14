using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using InstantaneousGram_UserProfile.Data;
using Microsoft.Extensions.Configuration;
using System;
using InstantaneousGram_UserProfile.Settings;
using InstantaneousGram_UserProfile.Managers;
using InstantaneousGram_UserProfile.Repositories;
using InstantaneousGram_UserProfile.Services;

var builder = WebApplication.CreateBuilder(args);


// Add Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
          builder =>
          {
              builder.WithOrigins(
                  "http://localhost:3000",
                  "http://localhost:5500")
                     .AllowAnyHeader()
                     .AllowAnyMethod();
          });
});

//Add dbContext
// Configure Entity Framework and SQL Server
builder.Services.AddDbContext<InstantaneousGramDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

string hostName = builder.Configuration.GetValue<string>("RabbitMQ:HostName");
int port = int.Parse(builder.Configuration.GetValue<string>("RabbitMQ:Port"));
string userName = builder.Configuration.GetValue<string>("RabbitMQ:UserName");
string password = builder.Configuration.GetValue<string>("RabbitMQ:Password");
var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
builder.Services.AddSingleton(rabbitMQSettings);
builder.Services.AddScoped<RabbitMQManager>();

// Register services and repositories
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("MyPolicy"); // Use CORS in all environments
app.MapControllers();

app.Run();
