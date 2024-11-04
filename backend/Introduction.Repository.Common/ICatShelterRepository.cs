using Introduction.Common;
using Introduction.Model;

namespace Introduction.Repository.Common
{
    public interface ICatShelterRepository
    {
        Task<List<CatShelter>> GetCatSheltersAsync(CatShelterFilter catShelterFilter, CatFilter catFilter, Paging paging, Sorting sorting);

        Task<List<CatShelter>> GetCatSheltersWithoutCatsAsync();

        Task<CatShelter?> GetCatShelterByIdAsync(Guid id);

        Task<bool> InsertCatShelterAsync(CatShelter catShelter);

        Task<bool> UpdateCatShelterByIdAsync(CatShelter catShelter);

        Task<bool> DeleteCatShelterByIdAsync(Guid id);
    }
}
