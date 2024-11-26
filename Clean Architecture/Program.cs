using Clean_Architecture.Application.UseCases;
using Clean_Architecture.Application.Validations;
using Clean_Architecture.Domain.Interfaces;
using Clean_Architecture.Infrastructure.Data;
using Clean_Architecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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
