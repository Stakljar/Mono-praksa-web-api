﻿using System.ComponentModel.DataAnnotations;

namespace Introduction.Model
{
    public class Cat
    {
        [Key]
        public Guid? Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        public int? Age { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Color { get; set; }

        public DateOnly? ArrivalDate { get; set; }

        public Guid? CatShelterId { get; set; }
    }
}
