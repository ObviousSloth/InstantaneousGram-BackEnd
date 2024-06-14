

using InstantaneousGram_ContentManagement.Settings;
using InstantaneousGram_ContentManagement.Managers;
using Instantaneousgram_ContentManagement.Data;
using InstantaneousGram_ContentManagement.Repositories;
using InstantaneousGram_ContentManagement.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register RabbitMQService with connection details
string hostName = builder.Configuration.GetValue<string>("RabbitMQ:HostName");
int port = int.Parse(builder.Configuration.GetValue<string>("RabbitMQ:Port"));
string userName = builder.Configuration.GetValue<string>("RabbitMQ:UserName");
string password = builder.Configuration.GetValue<string>("RabbitMQ:Password");
var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
builder.Services.AddSingleton(rabbitMQSettings);
builder.Services.AddScoped<RabbitMQManager>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework and SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services and repositories
builder.Services.AddScoped<IContentManagementRepository, ContentManagementRepository>();
builder.Services.AddScoped<IContentManagementService, ContentManagementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
