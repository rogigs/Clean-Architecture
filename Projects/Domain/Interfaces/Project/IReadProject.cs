using Projects.Application.Exceptions;
using Projects.Domain.Entities;

namespace Projects.Domain.Interfaces
{
    public interface IReadProject : IUseCase<Guid, Project?, ProjectException>;



}
