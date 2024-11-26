using Users.Application.Exceptions;
using Users.Application.UseCases.DTO;
using Users.Domain.Entities;

namespace Users.Domain.Interfaces
{
    public interface IUpdateUser : IUseCase<UserUpdateDTO, User?, UserException>;
}
