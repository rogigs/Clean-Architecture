using Projects.Application.Exceptions;
using Projects.Application.UseCases.DTO;
using Projects.Domain.Entities;

namespace Projects.Domain.Interfaces
{

    public interface IReadProjects : IUseCase<PaginationDTO, IEnumerable<Project>, ProjectException>;
}
