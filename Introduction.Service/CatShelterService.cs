using Introduction.Model;
using Introduction.Repository;
using Introduction.Service.Common;

namespace Introduction.Service
{
    public class CatShelterService : ICatShelterService
    {
        private readonly CatShelterRepository catShelterRepository = new();

        public async Task<List<CatShelter>?> GetCatSheltersAsync(string name = "", string location = "",
            DateOnly? createdAtDateAfter = null, DateOnly? createdAtDateBefore = null)
        {
            return await catShelterRepository.GetCatSheltersAsync(name, location, createdAtDateAfter, createdAtDateBefore);
        }

        public async Task<CatShelter?> GetCatShelterAsync(Guid id)
        {
            return await catShelterRepository.GetCatShelterByIdAsync(id);
        }

        public async Task<bool> PostCatShelterAsync(CatShelterAddModel catShelterAddModel)
        {
            return await catShelterRepository.InsertCatShelterAsync(catShelterAddModel);
        }

        public async Task<bool> PutCatShelterAsync(Guid id, CatShelterUpdateModel catShelterUpdateModel)
        {
            return await catShelterRepository.UpdateCatShelterByIdAsync(id, catShelterUpdateModel);
        }

        public async Task<bool> DeleteCatShelterAsync(Guid id)
        {
            return await catShelterRepository.DeleteCatShelterByIdAsync(id);
        }
    }
}
