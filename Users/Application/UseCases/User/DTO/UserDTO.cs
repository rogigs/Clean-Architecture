﻿
namespace Users.Application.UseCases.DTO
{
    public record UserDTO(string Name, string Email, string Password);
    public record UserUpdateDTO(string? Name, string? Email, string? NewEmail, string? Password, string? NewPassword);
}