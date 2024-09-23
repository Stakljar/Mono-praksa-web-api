using System;

namespace Introduction.Common
{
    public class CatFilter
    {
        public required string Name { get; set; }

        public int? AgeAbove { get; set; }

        public int? AgeBelow { get; set; }

        public required string Color { get; set; }

        public DateOnly? ArrivalDateAfter { get; set; }

        public DateOnly? ArrivalDateBefore { get; set; }
    }
}
