
using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Domain.Interfaces
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
