using Introduction.Common;
using Introduction.Model;
using Introduction.Repository.Common;
using Introduction.Service.Common;

namespace Introduction.Service
{
    public class CatService : ICatService
    {
        private readonly ICatRepository _catRepository;

        public CatService(ICatRepository catRepository)
        {
            _catRepository = catRepository;
        }

        public async Task<List<Cat>?> GetCatsAsync(CatFilter catFilter, Paging paging, Sorting sorting)
        {
            return await _catRepository.GetCatsAsync(catFilter, paging, sorting);
        }

        public async Task<Cat?> GetCatAsync(Guid id)
        {
            return await _catRepository.GetCatByIdAsync(id);
        }

        public async Task<bool> PostCatAsync(Cat cat)
        {
            return await _catRepository.InsertCatAsync(cat);
        }

        public async Task<bool> PutCatAsync(Cat cat)
        {
            return await _catRepository.UpdateCatByIdAsync(cat);
        }

        public async Task<bool> DeleteCatAsync(Guid id)
        {
            return await _catRepository.DeleteCatByIdAsync(id);
        }
    }
}
