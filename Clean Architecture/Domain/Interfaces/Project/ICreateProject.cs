
using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Domain.Interfaces
{

    public interface ICreateProject
    {
        Task<Project> ExecuteAsync(ProjectDTO projectDTO);
    }
}
