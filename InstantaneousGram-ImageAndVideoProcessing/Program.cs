using CloudinaryDotNet;
using dotenv.net;
using InstantaneousGram_ImageAndVideoProcessing.Managers;
using InstantaneousGram_ImageAndVideoProcessing.Settings;
using Microsoft.Azure.Cosmos;
using InstantaneousGram_ImageAndVideoProcessing.Services;
using InstantaneousGram_ImageAndVideoProcessing.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        builder =>
        {
            builder.WithOrigins(
                "http://localhost:3000",
                "http://localhost:5502")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Register RabbitMQService with connection details
string hostName = builder.Configuration.GetValue<string>("RabbitMQ:HostName");
int port = int.Parse(builder.Configuration.GetValue<string>("RabbitMQ:Port"));
string userName = builder.Configuration.GetValue<string>("RabbitMQ:UserName");
string password = builder.Configuration.GetValue<string>("RabbitMQ:Password");
var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
builder.Services.AddSingleton(rabbitMQSettings);
builder.Services.AddScoped<RabbitMQManager>();

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Set up Cloudinary credentials
DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"))
{
    Api = { Secure = true }
};
builder.Services.AddSingleton(cloudinary);

// Add Cosmos DB service
builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration["CosmosDb:ConnectionString"];
    return new CosmosClient(connectionString);
});

builder.Services.AddSingleton<CosmosDbService>();
builder.Services.AddTransient<ImageAndVideoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("MyPolicy");

app.MapControllers();

app.Run();
