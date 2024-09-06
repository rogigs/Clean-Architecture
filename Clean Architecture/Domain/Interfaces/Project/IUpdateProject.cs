using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Domain.Interfaces
{
    public interface IUpdateProject : IUseCase<ProjectUpdateDTO, Project?>;
}
