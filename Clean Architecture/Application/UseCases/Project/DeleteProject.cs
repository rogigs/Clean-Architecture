using Clean_Architecture.Application.Exceptions;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;

namespace Clean_Architecture.Application.UseCases
{
    internal sealed class DeleteProject(IProjectRepository projectRepository) : IDeleteProject
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<(ProjectException?, Project? )> ExecuteAsync(Guid projectId)
        {
            try
            {
                return (null, await _projectRepository.Delete(projectId));
            }
            catch (Exception ex) {
                return (new ProjectException("An error occurred while deleting the project.", ex), null);
            }
        }
    }
}
