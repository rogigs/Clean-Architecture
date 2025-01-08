namespace Projects.Application.UseCases.DTO
{
    public record PaginationDTO()
    {
        public required int Take { get; init; } = 10;
        public required int Skip { get; init; } = 0;
    }
}
