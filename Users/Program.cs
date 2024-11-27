using Microsoft.EntityFrameworkCore;
using Users.Application.UseCases;
using Users.Application.Validations;
using Users.Domain.Interfaces;
using Users.Infrastructure.Data;
using Users.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
       .LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<ICreateUser, CreateUser>();
builder.Services.AddScoped<IReadUser, ReadUser>();
builder.Services.AddScoped<IReadUsers, ReadUsers>();
builder.Services.AddScoped<IDeleteUser, DeleteUser>();
builder.Services.AddScoped<IUpdateUser, UpdateUser>();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(
        new NamespaceRoutingConvention());
});


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
