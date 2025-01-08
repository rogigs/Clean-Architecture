using System.Text.Json.Serialization;
using Projects.Application.Validations;
using Projects.Domain.Interfaces;
using Projects.Infrastructure.Data;
using Projects.Infrastructure.MessageQueue;
using Projects.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Projects.Application.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ICreateProject, CreateProject>();
builder.Services.AddScoped<IReadProject, ReadProject>();
builder.Services.AddScoped<IReadProjects, ReadProjects>();
builder.Services.AddScoped<IDeleteProject, DeleteProject>();
builder.Services.AddScoped<IUpdateProject, UpdateProject>();
builder.Services.AddScoped<RabbitMQConsumerService>();

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(
        new NamespaceRoutingConvention());
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
;

// TODO: app should run if rabbitmq service dropped
builder.Services.AddHostedService<RabbitMQConsumerService>();


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
