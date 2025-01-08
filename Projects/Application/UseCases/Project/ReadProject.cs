using Projects.Application.Exceptions;
using Projects.Domain.Entities;
using Projects.Domain.Interfaces;

namespace Projects.Application.UseCases
{
    internal sealed class ReadProject(IProjectRepository projectRepository) : IReadProject
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

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
