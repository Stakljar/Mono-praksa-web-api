
namespace Introduction.WebAPI.RestModels
{
    public class CatUpdateModel
    {
        public int? Age { get; set; }

        public string? Color { get; set; }

        public DateOnly? ArrivalDate { get; set; }

        public Guid? CatShelterId { get; set; }
    }
}
