using Projects.Application.Exceptions;
using Projects.Application.UseCases.DTO;
using Projects.Domain.Entities;
using Projects.Domain.Interfaces;

namespace Projects.Application.UseCases
{
    internal sealed class ReadProjects(IProjectRepository projectRepository) : IReadProjects
    {

        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<(ProjectException?, IEnumerable<Project>?)> ExecuteAsync(PaginationDTO pagination)
        {
            try
            {
                //TODO: Add cache 
                return (null, await _projectRepository.GetAll(pagination));
            }
            catch (Exception ex)
            {
                return (new ProjectException("An error occurred while getting projects.", ex), null);
            }
        }
    }
}
