using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using InstantaneousGram_UserProfile.Data;
using Microsoft.Extensions.Configuration;
using System;
using InstantaneousGram_UserProfile.Settings;
using InstantaneousGram_UserProfile.Managers;

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
//builder.Services.AddDbContext<InstantaneousGram_UserProfileContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("InstantaneousGram_UserProfileContext") ?? throw new InvalidOperationException("Connection string 'InstantaneousGram_UserProfileContext' not found.")));
builder.Services.AddDbContext<InstantaneousGram_UsersContextSQLite>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("InstantaneousGram_UserContext") ?? throw new InvalidOperationException("Connection string 'InstantaneousGram_UserContext' not found.")));
Console.WriteLine("DATABASE: " + builder.Configuration.GetConnectionString("InstantaneousGram_UserContext"));
Console.WriteLine("DATABASE: " + builder.Configuration.GetConnectionString("InstantaneousGram_UserContext"));
Console.WriteLine("DATABASE: " + builder.Configuration.GetConnectionString("InstantaneousGram_UserContext"));
Console.WriteLine("DATABASE: " + builder.Configuration.GetConnectionString("InstantaneousGram_UserContext"));
Console.WriteLine("DATABASE: " + builder.Configuration.GetConnectionString("InstantaneousGram_UserContext"));

string hostName = builder.Configuration.GetValue<string>("RabbitMQ:HostName");
int port = int.Parse(builder.Configuration.GetValue<string>("RabbitMQ:Port"));
string userName = builder.Configuration.GetValue<string>("RabbitMQ:UserName");
string password = builder.Configuration.GetValue<string>("RabbitMQ:Password");
Console.WriteLine("hostname: " + hostName + " port: " + port + " userName: " + userName + "password: " + password);
Console.WriteLine("hostname: " + hostName + " port: " + port + " userName: " + userName + "password: " + password);
Console.WriteLine("hostname: " + hostName + " port: " + port + " userName: " + userName + "password: " + password);
Console.WriteLine("hostname: " + hostName + " port: " + port + " userName: " + userName + "password: " + password);
Console.WriteLine("hostname: " + hostName + " port: " + port + " userName: " + userName + "password: " + password);
var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
builder.Services.AddSingleton(rabbitMQSettings);
builder.Services.AddScoped<RabbitMQManager>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations for SQLite DbContext
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<InstantaneousGram_UsersContextSQLite>();
dbContext.Database.Migrate();

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
