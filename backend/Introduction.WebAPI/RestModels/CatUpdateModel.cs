
using System.ComponentModel.DataAnnotations;

namespace Introduction.WebAPI.RestModels
{
    public class CatUpdateModel
    {
        public string? Name { get; set; }

        [Range(0, 35, ErrorMessage = "Age must be between 0 and 35.")]
        public int? Age { get; set; }

        public string? Color { get; set; }

        public DateOnly? ArrivalDate { get; set; }

        public Guid? ShelterId { get; set; }
    }
}
