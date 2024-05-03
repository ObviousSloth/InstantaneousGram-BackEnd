using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using InstantaneousGram_UserProfile.Data;
using Microsoft.Extensions.Configuration;
using System;

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
builder.Services.AddDbContext<InstantaneousGram_UserProfileContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InstantaneousGram_UserProfileContext") ?? throw new InvalidOperationException("Connection string 'InstantaneousGram_UserProfileContext' not found.")));
builder.Services.AddDbContext<InstantaneousGram_UsersContextSQLite>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("InstantaneousGram_UserContext") ?? throw new InvalidOperationException("Connection string 'InstantaneousGram_UserProfileContext' not found.")));
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
