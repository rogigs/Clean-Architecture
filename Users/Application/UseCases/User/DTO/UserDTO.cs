
namespace Users.Application.UseCases.DTO
{
    public record UserDTO(string Name, string Email);
    public record UserUpdateDTO(string? Name, string Email);
}