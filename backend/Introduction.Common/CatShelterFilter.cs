
namespace Introduction.Common
{
    public class CatShelterFilter
    {
        public string? Name { get; set; }

        public string? Location { get; set; }

        public DateOnly? EstablishedAtAfter { get; set; }

        public DateOnly? EstablishedAtBefore { get; set; }

    }
}
