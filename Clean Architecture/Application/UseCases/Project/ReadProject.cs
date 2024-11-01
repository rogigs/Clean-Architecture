using Clean_Architecture.Application.Exceptions;
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

        public async Task<(ProjectException?, Project?)> ExecuteAsync(Guid projectId)
        {
            try
            {
                return (null, await _projectRepository.GetById(projectId));
            }
            catch (Exception ex)
            {
                return (new ProjectException("An error occurred while getting a project.", ex), null);
            }
        }
    }
}
