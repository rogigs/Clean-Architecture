using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Users.Domain.Entities
{
    [Index(nameof(UserId), nameof(Email))]
    public record User
    {
        [Key]
        public Guid UserId { get; init; } = Guid.NewGuid();

        [Required(ErrorMessage = "User name is required")]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "User email is required")]
        [EmailAddress(ErrorMessage = "The provided email is not valid.")]
        [StringLength(100)]
        public required string Email { get; set; }
    }
}
