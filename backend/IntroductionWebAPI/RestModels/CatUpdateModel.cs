
namespace Introduction.WebAPI.RestModels
{
    public class CatUpdateModel
    {
        public string? Name { get; set; }

        public int? Age { get; set; }

        public string? Color { get; set; }

        public DateOnly? ArrivalDate { get; set; }

        public Guid? ShelterId { get; set; }
    }
}
