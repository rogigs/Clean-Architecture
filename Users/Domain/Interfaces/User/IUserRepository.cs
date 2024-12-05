
using Users.Application.UseCases.DTO;
using Users.Domain.Entities;

namespace Users.Domain.Interfaces
{
    //TODO: inherit IRepository
    public interface IUserRepository
    {
        Task<User?> GetById(Guid userId);
        Task<IEnumerable<User>> GetAll(PaginationDTO paginationDTO);
        Task Add(User user, OutboxMessage outboxMessage);
        Task<User?> Update(UserUpdateDTO userDTO, Guid userId);
        Task<User?> Delete(Guid userId);
    }
}
