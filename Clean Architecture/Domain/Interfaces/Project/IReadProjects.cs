using Clean_Architecture.Application.Exceptions;
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Domain.Interfaces
{

    public interface IReadProjects : IUseCase<Pagination, IEnumerable<Project>, ProjectException>;
}
