using Users.Application.UseCases.DTO;
using Users.Application.Validations;
using Users.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Users.Web.Controllers
{
    [ApiController]
    public class UserController(
        ICreateUser createUser,
        IReadUser readUser,
        IReadUsers readUsers,
        IDeleteUser deleteUser,
        IUpdateUser updateUser) : ControllerBase, IUserController
    {
        private readonly ICreateUser _createUser = createUser;
        private readonly IReadUser _readUser = readUser;
        private readonly IReadUsers _readUsers = readUsers;
        private readonly IDeleteUser _deleteUser = deleteUser;
        private readonly IUpdateUser _updateUser = updateUser;

        [HttpPost(Name = "PostUser")]
        public async Task<IActionResult> PostAsync([FromBody] UserDTO UserDTO)
        {
            var (error, createdUser) = await _createUser.ExecuteAsync(UserDTO);

            return error == null ? CreatedAtAction(null, createdUser) : BadRequest(new { error.Message });
        }

        [HttpGet("{userId:guid}", Name = "GetUser")]
        public async Task<IActionResult> GetAsync(Guid userId)
        {
            var (error, user) = await _readUser.ExecuteAsync(userId);

            if (error != null) return BadRequest(new { error.Message });

            return user == null
                ? NotFound(new { Message = "User not found" })
                : Ok(user);
        }

        [HttpGet(Name = "GetAllUser")]
        [ValidatePaginationAttributes]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDTO paginationDTO)
        {
            var (error, users) = await _readUsers.ExecuteAsync(paginationDTO);

            return error == null ? Ok(users) : BadRequest(new { error.Message });
        }

        [HttpDelete("{userId:guid}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteAsync(Guid userId)
        {
            var (error, user) = await _deleteUser.ExecuteAsync(userId);

            if (error != null) return BadRequest(new { error.Message });

            return user == null
                ? NotFound(new { Message = "User not found" })
                : Ok(user);
        }

        [HttpPatch("{userId:guid}", Name = "UpdateUser")]
        [ValidateUpdateUserAttributes]
        public async Task<IActionResult> UpdateAsync([FromBody] UserUpdateDTO UserDTO, Guid userId)
        {
            var (error, User) = await _updateUser.ExecuteAsync(UserDTO, userId);

            if (error != null) return BadRequest(new { error.Message });

            return User == null
                ? NotFound(new { Message = "User not found" })
                : Ok(User);
        }
    }
}
