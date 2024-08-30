﻿using System.ComponentModel.DataAnnotations;

namespace Clean_Architecture.Domain.Entities
{
    public class Project
    {
        [Key]
        public Guid ProjectId { get; init; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime StartDate { get; init; } = DateTime.Now;

        public DateTime? EndDate { get; set; }
    }
}
