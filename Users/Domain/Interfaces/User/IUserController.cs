using Users.Application.UseCases.DTO;

namespace Users.Domain.Interfaces
{
    public interface IUserController : IController<UserDTO, UserUpdateDTO>;
}
