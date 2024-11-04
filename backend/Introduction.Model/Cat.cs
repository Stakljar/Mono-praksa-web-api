using System.ComponentModel.DataAnnotations;

namespace Introduction.Model
{
    public class Cat
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }

        public int? Age { get; set; }

        [MaxLength(50)]
        public string? Color { get; set; }

        public DateOnly? ArrivalDate { get; set; }

        public Guid? CatShelterId { get; set; }

        public CatShelter? CatShelter { get; set; }
    }
}
