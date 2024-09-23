using System.ComponentModel.DataAnnotations;

namespace Introduction.Model
{
    public class CatShelter
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(300)]
        public string? Name { get; set; }

        [MaxLength(400)]
        public string? Location { get; set; }

        public DateOnly? EstablishedAt { get; set; }

        public List<Cat> Cats { get; set; } = [];
    }
}
