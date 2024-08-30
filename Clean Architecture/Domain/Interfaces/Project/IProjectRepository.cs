
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> GetById(Guid projectId);
        Task<IEnumerable<Project>> GetAll();
        Task Add(Project project);
        Task Update(Project project);
        Task Delete(Guid projectId);
    }
}
