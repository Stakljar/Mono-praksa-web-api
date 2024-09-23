using Introduction.Common;
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

        public async Task<List<CatShelter>?> GetCatSheltersAsync(CatShelterFilter catShelterFilter, CatFilter catFilter, Paging paging, Sorting sorting)
        {
            return await _catShelterRepository.GetCatSheltersAsync(catShelterFilter, catFilter, paging, sorting);
        }

        public async Task<CatShelter?> GetCatShelterAsync(Guid id)
        {
            return await _catShelterRepository.GetCatShelterByIdAsync(id);
        }

        public async Task<bool> PostCatShelterAsync(CatShelter catShelter)
        {
            return await _catShelterRepository.InsertCatShelterAsync(catShelter);
        }

        public async Task<bool> PutCatShelterAsync(CatShelter catShelter)
        {
            return await _catShelterRepository.UpdateCatShelterByIdAsync(catShelter);
        }

        public async Task<bool> DeleteCatShelterAsync(Guid id)
        {
            return await _catShelterRepository.DeleteCatShelterByIdAsync(id);
        }
    }
}
