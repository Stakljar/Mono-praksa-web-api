namespace Introduction.WebAPI.RestModels
{
    public class CatShelterWithoutCatsGetModel
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Location { get; set; }

        public DateOnly EstablishedAt { get; set; }
    }
}
