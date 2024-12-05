using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Users.Domain.Entities
{
    [Index(nameof(OutboxMessageId))]
    public class OutboxMessage
    {
        [Key]
        public Guid OutboxMessageId { get; init; } = Guid.NewGuid();

        [Required(ErrorMessage = "OutboxMessage payload is required")]
        public required string Payload { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public bool Processed { get; set; } 
    }
}
