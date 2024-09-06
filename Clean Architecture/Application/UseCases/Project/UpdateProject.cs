using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Interfaces;

namespace Clean_Architecture.Application.UseCases
{
    public class UpdateProject : IUpdateProject
    {
        private readonly IProjectRepository _projectRepository;
        public UpdateProject(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project?> ExecuteAsync(ProjectUpdateDTO project)
        {
            throw new NotSupportedException("This method is not supported and should not be implemented.");

        }


        public async Task<Project?> ExecuteAsync(ProjectUpdateDTO projectDTO, Guid projectId)
        {

            return await _projectRepository.Update(projectDTO, projectId); ;
        }
    }
}
