using System.ComponentModel.DataAnnotations;

namespace Introduction.WebAPI.RestModels
{
    public class CatShelterAddModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public required string Location { get; set; }

        [Required(ErrorMessage = "Established at date is required.")]
        public DateOnly? EstablishedAt { get; set; }
    }
}
