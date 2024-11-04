using System.Runtime.CompilerServices;
using Clean_Architecture.Application.Exceptions;
using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;

[assembly: InternalsVisibleTo("Clean Architecture.Tests")]

namespace Clean_Architecture.Application.UseCases
{
    internal sealed class CreateProject(IProjectRepository projectRepository) : ICreateProject
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<(ProjectException?, Project?)> ExecuteAsync(ProjectDTO projectDTO)
        {
            try
            {
                var project = new Project
                {
                    Name = projectDTO.Name,
                    Description = projectDTO.Description,
                    EndDate = projectDTO.EndDate,
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
