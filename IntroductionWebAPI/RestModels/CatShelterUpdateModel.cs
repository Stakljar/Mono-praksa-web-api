namespace IntroductionWebAPI.RestModels
{
    public class CatShelterUpdateModel
    {
        public string? Name { get; set; }

        public string? Location { get; set; }

        public DateOnly? EstablishedAt { get; set; }
    }
}
