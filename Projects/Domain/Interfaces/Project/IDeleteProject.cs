using Projects.Application.Exceptions;
using Projects.Domain.Entities;

namespace Projects.Domain.Interfaces
{
    public interface IDeleteProject : IUseCase<Guid, Project?, ProjectException>;
}
