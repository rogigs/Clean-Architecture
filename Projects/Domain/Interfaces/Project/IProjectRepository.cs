using Projects.Application.UseCases.DTO;
using Projects.Domain.Entities;

namespace Projects.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project?> GetById(Guid projectId);
        Task<IEnumerable<Project>> GetAll(PaginationDTO paginationDTO);
        Task Add(Project project);
        Task<Project?> Update(ProjectUpdateDTO projectDTO, Guid projectId);
        Task<Project?> Delete(Guid projectId);
    }
}
