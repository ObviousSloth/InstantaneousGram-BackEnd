using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using InstantaneousGram_ContentManagement.Managers;
using InstantaneousGram_ContentManagement.Repositories;
using InstantaneousGram_ContentManagement.Services;
using Instantaneousgram_ContentManagement.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services to the container.
string hostName = builder.Configuration.GetValue<string>("RabbitMQ:HostName");
int port = int.Parse(builder.Configuration.GetValue<string>("RabbitMQ:Port"));
string userName = builder.Configuration.GetValue<string>("RabbitMQ:UserName");
string password = builder.Configuration.GetValue<string>("RabbitMQ:Password");

var factory = new ConnectionFactory() { HostName = hostName, Port = port, UserName = userName, Password = password };
var connection = factory.CreateConnection();

builder.Services.AddSingleton(connection);
builder.Services.AddSingleton<RabbitMQSubscriber>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IContentManagementRepository, ContentManagementRepository>();
builder.Services.AddScoped<IContentManagementService, ContentManagementService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var rabbitMQSubscriber = app.Services.GetRequiredService<RabbitMQSubscriber>();
rabbitMQSubscriber.Subscribe();

app.Run();
