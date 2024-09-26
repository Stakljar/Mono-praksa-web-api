
namespace Introduction.Common
{
    public class CatFilter
    {
        public string? Name { get; set; }

        public int? AgeAbove { get; set; }

        public int? AgeBelow { get; set; }

        public string? Color { get; set; }

        public DateOnly? ArrivalDateAfter { get; set; }

        public DateOnly? ArrivalDateBefore { get; set; }
    }
}
