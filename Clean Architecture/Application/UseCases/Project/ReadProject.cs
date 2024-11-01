using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;

namespace Clean_Architecture.Application.UseCases
{
    internal sealed class ReadProject : IReadProject
    {
        private readonly IProjectRepository _projectRepository;
        public ReadProject(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project?> ExecuteAsync(Guid projectId)
        {
            return await _projectRepository.GetById(projectId); ;
        }
    }
}
