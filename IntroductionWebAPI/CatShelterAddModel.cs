using System.ComponentModel.DataAnnotations;

namespace IntroductionWebAPI
{
    public class CatShelterAddModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public required string Location { get; set; }

        public DateOnly? CreatedAt { get; set; }
    }
}
