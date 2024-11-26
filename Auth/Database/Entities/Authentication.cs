using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Auth.Database.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public record Authentication
    {
        [Key]
        [Required(ErrorMessage = "Auth email is required")]
        [EmailAddress(ErrorMessage = "The provided email is not valid.")]
        [StringLength(100)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Auth password is required")]
        [StringLength(100)]
        public required string Password { get; set; }

    }
}
