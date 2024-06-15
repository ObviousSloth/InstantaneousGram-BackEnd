using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using InstantaneousGram_LikesAndComments.Repositories;
using InstantaneousGram_LikesAndComments.Services;
using InstantaneousGram_LikesAndComments.Settings;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using InstantaneousGram_LikesAndComments.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<CosmosDbSettings>(builder.Configuration.GetSection("CosmosDb"));
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton(s =>
{
    var settings = s.GetRequiredService<IOptions<CosmosDbSettings>>().Value;
    return new CosmosClient(settings.ConnectionString);
});

builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Add RabbitMQ services
builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
{
    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration["RabbitMQ:HostName"],
        Port = int.Parse(builder.Configuration["RabbitMQ:Port"]),
        UserName = builder.Configuration["RabbitMQ:UserName"],
        Password = builder.Configuration["RabbitMQ:Password"]
    };
    return factory;
});

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = sp.GetRequiredService<IConnectionFactory>();
    return factory.CreateConnection();
});

builder.Services.AddSingleton<RabbitMQListener>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<RabbitMQListener>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

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
