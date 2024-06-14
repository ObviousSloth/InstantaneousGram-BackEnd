using InstantaneousGram_ImageAndVideoProcessing.Managers;
using InstantaneousGram_ImageAndVideoProcessing.Repositories;
using InstantaneousGram_ImageAndVideoProcessing.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
if (string.IsNullOrEmpty(cloudinaryUrl))
{
    throw new Exception("CLOUDINARY_URL environment variable is not set.");
}

var account = new CloudinaryDotNet.Cloudinary(cloudinaryUrl);
builder.Services.AddSingleton(account);

builder.Services.AddSingleton(s => new CosmosClient(builder.Configuration["CosmosDb:ConnectionString"]));
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IImageAndVideoService, ImageAndVideoService>();
builder.Services.AddScoped<IImageAndVideoRepository, ImageAndVideoRepository>();

// Add RabbitMQ connection factory and connection
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration["RabbitMQ:HostName"],
        Port = int.Parse(builder.Configuration["RabbitMQ:Port"]),
        UserName = builder.Configuration["RabbitMQ:UserName"],
        Password = builder.Configuration["RabbitMQ:Password"]
    };
    return factory.CreateConnection();
});

builder.Services.AddSingleton<RabbitMQManager>();
builder.Services.AddScoped<UserDeletionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Subscribe to RabbitMQ user deletion events
var rabbitMQManager = app.Services.GetRequiredService<RabbitMQManager>();

rabbitMQManager.SubscribeToUserDeletedEvent(async (userId) =>
{
    using var scope = app.Services.CreateScope();
    var userDeletionService = scope.ServiceProvider.GetRequiredService<UserDeletionService>();
    await userDeletionService.HandleUserDeletedAsync(userId);
});

app.Run();
