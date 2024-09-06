using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;

namespace Clean_Architecture.Application.UseCases
{
    public class DeleteProject : IDeleteProject
    {
        private readonly IProjectRepository _projectRepository;
        public DeleteProject(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project?> ExecuteAsync(Guid projectId)
        {
            return await _projectRepository.Delete(projectId);
        }
    }
}
