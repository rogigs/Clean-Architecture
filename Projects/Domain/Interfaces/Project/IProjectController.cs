using Projects.Application.UseCases.DTO;

namespace Projects.Domain.Interfaces
{
    public interface IProjectController : IController<ProjectDTO, ProjectUpdateDTO>;
}
