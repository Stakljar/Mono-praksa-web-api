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

        public async Task<List<Cat>?> GetCatsAsync(string name = "", int? age = null, string color = "",
            DateOnly? arrivalDateAfter = null, DateOnly? arrivalDateBefore = null)
        {
            return await _catRepository.GetCatsAsync(name, age, color, arrivalDateAfter, arrivalDateBefore);
        }

        public async Task<Cat?> GetCatAsync(Guid id)
        {
            return await _catRepository.GetCatByIdAsync(id);
        }

        public async Task<bool> PostCatAsync(CatAddModel catAddModel)
        {
            return await _catRepository.InsertCatAsync(catAddModel);
        }

        public async Task<bool> PutCatAsync(Guid id, CatUpdateModel catUpdateModel)
        {
            return await _catRepository.UpdateCatByIdAsync(id, catUpdateModel);
        }

        public async Task<bool> DeleteCatAsync(Guid id)
        {
            return await _catRepository.DeleteCatByIdAsync(id);
        }
    }
}
