using Introduction.Model;
using System.ComponentModel.DataAnnotations;

namespace IntroductionWebAPI.RestModels
{
    public class CatShelterGetModel
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Location { get; set; }

        public DateOnly EstablishedAt { get; set; }

        public List<CatGetModel> Cats { get; set; } = [];
    }
}
