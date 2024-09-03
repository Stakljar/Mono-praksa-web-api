namespace IntroductionWebAPI
{
    public class Cat(long id, string name, int age, string color, DateOnly arrivalDate)
    {
        public long Id { get; set; } = id;

        public string Name { get; set; } = name;

        public int Age { get; set; } = age;

        public string Color { get; set; } = color;

        public DateOnly ArrivalDate { get; set; } = arrivalDate;
    }
}
