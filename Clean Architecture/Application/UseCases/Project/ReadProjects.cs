using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;

namespace Clean_Architecture.Application.UseCases
{
    public class ReadProjects : IReadProjects
    {

        private readonly IProjectRepository _projectRepository;
        public ReadProjects(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> ExecuteAsync(Pagination pagination)
        {
            return await _projectRepository.GetAll(pagination); ;
        }
    }
}
