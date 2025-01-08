using Projects.Application.Exceptions;
using Projects.Application.UseCases.DTO;
using Projects.Domain.Entities;
using Projects.Domain.Interfaces;

namespace Projects.Application.UseCases
{
    internal sealed class UpdateProject(IProjectRepository projectRepository) : IUpdateProject
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public Task<(ProjectException?, Project?)> ExecuteAsync(ProjectUpdateDTO project)
        {
            throw new NotSupportedException("This method is not supported and should not be implemented.");

        }

        public async Task<(ProjectException?, Project?)> ExecuteAsync(ProjectUpdateDTO projectUpdateDTO, Guid projectId)
        {
            try
            {
                return (null, await _projectRepository.Update(projectUpdateDTO, projectId));
            }
            catch (Exception ex)
            {
                return (new ProjectException("An error occurred while uploading a project.", ex), null);
            }
        }
    }
}
