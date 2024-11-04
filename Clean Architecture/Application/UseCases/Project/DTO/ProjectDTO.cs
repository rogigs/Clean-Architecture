
namespace Clean_Architecture.Application.UseCases.DTO
{


    public record ProjectDTO(string Name, string? Description, DateTime? EndDate);
    public record ProjectUpdateDTO
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public DateTime? EndDate { get; init; }

        public ProjectUpdateDTO(string? name, string? description, DateTime? endDate)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(description) && !endDate.HasValue)
            {
                throw new ArgumentException("At least one of Name, Description, or EndDate must be provided.");
            }

            Name = name;
            Description = description;
            EndDate = endDate;
        }
    }

}