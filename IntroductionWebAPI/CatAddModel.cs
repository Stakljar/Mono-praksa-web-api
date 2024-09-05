using System.ComponentModel.DataAnnotations;

namespace IntroductionWebAPI
{
    public class CatAddModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Color is required.")]
        public required string Color { get; set; }

        public DateOnly? ArrivalDate { get; set; }
    }
}
