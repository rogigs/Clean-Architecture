using Users.Application.Exceptions;
using Users.Application.UseCases.DTO;
using Users.Domain.Entities;
using Users.Domain.Interfaces;

namespace Users.Application.UseCases
{
    internal sealed class ReadUsers(IUserRepository userRepository) : IReadUsers
    {

        private readonly IUserRepository _userRepository = userRepository;

        public async Task<(UserException?, IEnumerable<User>?)> ExecuteAsync(PaginationDTO pagination)
        {
            try
            {
                //TODO: Add cache 
                return (null, await _userRepository.GetAll(pagination));
            }
            catch (Exception ex)
            {
                return (new UserException("An error occurred while getting users.", ex), null);
            }
        }
    }
}
