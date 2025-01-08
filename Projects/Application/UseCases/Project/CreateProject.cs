using Projects.Application.Exceptions;
using Projects.Application.UseCases.DTO;
using Projects.Domain.Entities;
using Projects.Domain.Interfaces;

namespace Projects.Application.UseCases
{
    internal sealed class CreateProject(IProjectRepository projectRepository) : ICreateProject
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<(ProjectException?, Project?)> ExecuteAsync(ProjectDTO projectDTO)
        {
            try
            {
                Project project = new()
                {
                    Name = projectDTO.Name,
                    Description = projectDTO.Description,
                    EndDate = projectDTO.EndDate,
                    UsersId = projectDTO.UsersId
                };

                await _projectRepository.Add(project);
                return (null, project);
            }
            catch (Exception ex)
            {
                return (new ProjectException("An error occurred while creating a project.", ex), null);
            }
        }
    }
}
