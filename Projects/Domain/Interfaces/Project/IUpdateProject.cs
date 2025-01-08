using Projects.Application.Exceptions;
using Projects.Application.UseCases.DTO;
using Projects.Domain.Entities;

namespace Projects.Domain.Interfaces
{
    public interface IUpdateProject : IUseCase<ProjectUpdateDTO, Project?, ProjectException>;
}
