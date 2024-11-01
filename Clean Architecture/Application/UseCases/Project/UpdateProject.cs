using Clean_Architecture.Application.Exceptions;
using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;

namespace Clean_Architecture.Application.UseCases
{
    internal sealed class UpdateProject(IProjectRepository projectRepository) : IUpdateProject
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public Task<(ProjectException?, Project?)> ExecuteAsync(ProjectUpdateDTO project)
        {
            throw new NotSupportedException("This method is not supported and should not be implemented.");

        }

        public async Task<(ProjectException?, Project?)> ExecuteAsync(ProjectUpdateDTO projectDTO, Guid projectId)
        {
            try
            {
                return (null, await _projectRepository.Update(projectDTO, projectId));
            }
            catch (Exception ex)
            {
                return (new ProjectException("An error occurred while uploading a project.", ex), null);
            }
        }
    }
}
