using Clean_Architecture.Application.Exceptions;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;

namespace Clean_Architecture.Application.UseCases
{
    internal sealed class ReadProjects : IReadProjects
    {

        private readonly IProjectRepository _projectRepository;
        public ReadProjects(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<(ProjectException?, IEnumerable<Project>?)> ExecuteAsync(Pagination pagination)
        {
            try
            {
                return (null, await _projectRepository.GetAll(pagination));
            }
            catch (Exception ex)
            {
                return (new ProjectException("An error occurred while getting a projects.", ex), null);
            }
        }
    }
}
