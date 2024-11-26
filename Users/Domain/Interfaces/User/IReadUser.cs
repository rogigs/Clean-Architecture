using Users.Application.Exceptions;
using Users.Domain.Entities;

namespace Users.Domain.Interfaces
{
    public interface IReadUser : IUseCase<Guid, User?, UserException>;
}
