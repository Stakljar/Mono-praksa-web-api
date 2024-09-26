
namespace Introduction.WebAPI.RestModels
{
    public class CatGetModel
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public int Age { get; set; }

        public required string Color { get; set; }

        public DateOnly? ArrivalDate { get; set; }

        public Guid? CatShelterId { get; set; }

        public string? CatShelterName { get; set; }
    }
}
