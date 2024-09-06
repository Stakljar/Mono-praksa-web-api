using Introduction.Model;
using Introduction.Repository;
using Introduction.Service.Common;

namespace Introduction.Service
{
    public class CatShelterService : ICatShelterService
    {
        private readonly CatShelterRepository catShelterRepository = new();

        public List<CatShelter>? GetCatShelters(string name = "", string location = "", DateOnly? createdAtDateAfter = null, DateOnly? createdAtDateBefore = null)
        {
            return catShelterRepository.GetCatShelters(name, location, createdAtDateAfter, createdAtDateBefore);
        }

        public CatShelter? GetCatShelter(Guid id)
        {
            return catShelterRepository.GetCatShelterById(id);
        }

        public bool PostCatShelter(CatShelterAddModel catShelterAddModel)
        {
            return catShelterRepository.InsertCatShelter(catShelterAddModel);
        }

        public bool PutCatShelter(Guid id, CatShelterUpdateModel catShelterUpdateModel)
        {
            return catShelterRepository.UpdateCatShelterById(id, catShelterUpdateModel);
        }

        public bool DeleteCatShelter(Guid id)
        {
            return catShelterRepository.DeleteCatShelterById(id);
        }
    }
}
