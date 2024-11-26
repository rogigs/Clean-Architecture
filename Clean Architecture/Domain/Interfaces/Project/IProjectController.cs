using Clean_Architecture.Application.UseCases.DTO;

namespace Clean_Architecture.Domain.Interfaces
{
    //TODO: Add more methods in IController
    public interface IProjectController : IController<ProjectDTO, ProjectUpdateDTO>;
}
