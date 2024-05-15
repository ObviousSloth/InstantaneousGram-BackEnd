using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using InstantaneousGram_ImageAndVideoProcessing.Managers;
using InstantaneousGram_ImageAndVideoProcessing.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add Cors
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

// Set my Cloudinary credentials
//=================================

DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));

Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
cloudinary.Api.Secure = true;

builder.Services.AddSingleton(cloudinary);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Apply CORS policy to all requests
app.UseCors("MyPolicy");

app.MapControllers();

app.Run();
