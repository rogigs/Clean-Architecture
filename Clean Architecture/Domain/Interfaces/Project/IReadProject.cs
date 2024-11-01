using Clean_Architecture.Application.Exceptions;
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Domain.Interfaces
{
    public interface IReadProject : IUseCase<Guid, Project?, ProjectException>;



}
