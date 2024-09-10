using Introduction.Model;
using Introduction.Repository.Common;
using Introduction.Service.Common;

namespace Introduction.Service
{
    public class CatShelterService : ICatShelterService
    {
        private readonly ICatShelterRepository _catShelterRepository;

        public CatShelterService(ICatShelterRepository catShelterRepository)
        {
            _catShelterRepository = catShelterRepository;
        }

        public async Task<List<CatShelter>?> GetCatSheltersAsync(string name = "", string location = "",
            DateOnly? createdAtDateAfter = null, DateOnly? createdAtDateBefore = null)
        {
            return await _catShelterRepository.GetCatSheltersAsync(name, location, createdAtDateAfter, createdAtDateBefore);
        }

        public async Task<CatShelter?> GetCatShelterAsync(Guid id)
        {
            return await _catShelterRepository.GetCatShelterByIdAsync(id);
        }

        public async Task<bool> PostCatShelterAsync(CatShelterAddModel catShelterAddModel)
        {
            return await _catShelterRepository.InsertCatShelterAsync(catShelterAddModel);
        }

        public async Task<bool> PutCatShelterAsync(Guid id, CatShelterUpdateModel catShelterUpdateModel)
        {
            return await _catShelterRepository.UpdateCatShelterByIdAsync(id, catShelterUpdateModel);
        }

        public async Task<bool> DeleteCatShelterAsync(Guid id)
        {
            return await _catShelterRepository.DeleteCatShelterByIdAsync(id);
        }
    }
}
