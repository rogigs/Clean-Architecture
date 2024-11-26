using Users.Application.Exceptions;
using Users.Domain.Entities;

namespace Users.Domain.Interfaces
{
    public interface IDeleteUser : IUseCase<Guid, User?, UserException>;
}
