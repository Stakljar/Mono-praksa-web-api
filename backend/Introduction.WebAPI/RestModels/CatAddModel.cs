﻿using System.ComponentModel.DataAnnotations;

namespace Introduction.WebAPI.RestModels
{
    public class CatAddModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(0, 35, ErrorMessage = "Age must be between 0 and 35.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Color is required.")]
        public required string Color { get; set; }

        [Required(ErrorMessage = "Arrival date is required.")]
        public DateOnly? ArrivalDate { get; set; }

        public Guid? ShelterId { get; set; }
    }
}
