using Introduction.Model;
using Introduction.Repository;

namespace Introduction.Service
{
    public class CatService
    {
        private readonly CatRepository catRepository = new();

        public async Task<List<Cat>?> GetCatsAsync(string name = "", int? age = null, string color = "",
            DateOnly? ArrivalDateAfter = null, DateOnly? ArrivalDateBefore = null)
        {
            return await catRepository.GetCatsAsync(name, age, color, ArrivalDateAfter, ArrivalDateBefore);
        }

        public async Task<Cat?> GetCatAsync(Guid id)
        {
            return await catRepository.GetCatByIdAsync(id);
        }

        public async Task<bool> PostCatAsync(CatAddModel catAddModel)
        {
            return await catRepository.InsertCatAsync(catAddModel);
        }

        public async Task<bool> PutCatAsync(Guid id, CatUpdateModel catUpdateModel)
        {
            return await catRepository.UpdateCatByIdAsync(id, catUpdateModel);
        }

        public async Task<bool> DeleteCatAsync(Guid id)
        {
            return await catRepository.DeleteCatByIdAsync(id);
        }
    }
}
