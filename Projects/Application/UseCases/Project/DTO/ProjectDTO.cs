namespace Projects.Application.UseCases.DTO
{
    public record ProjectDTO(string Name, string? Description, DateTime? EndDate, List<Guid> UsersId);
    public record ProjectUpdateDTO(string? Name, string? Description, DateTime? EndDate, List<Guid>? UsersId);

}