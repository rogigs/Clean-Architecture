using Projects.Application.Exceptions;
using Projects.Domain.Entities;
using Projects.Domain.Interfaces;

namespace Projects.Application.UseCases
{
    internal sealed class DeleteProject(IProjectRepository projectRepository) : IDeleteProject
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<(ProjectException?, Project?)> ExecuteAsync(Guid projectId)
        {
            try
            {
                return (null, await _projectRepository.Delete(projectId));
            }
            catch (Exception ex)
            {
                return (new ProjectException("An error occurred while deleting a project.", ex), null);
            }
        }
    }
}
