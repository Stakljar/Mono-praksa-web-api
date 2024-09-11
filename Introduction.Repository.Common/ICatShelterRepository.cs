using Introduction.Common;
using Introduction.Model;

namespace Introduction.Repository.Common
{
    public interface ICatShelterRepository
    {
        Task<List<CatShelter>?> GetCatSheltersAsync(CatShelterFilter catShelterFilter, CatFilter catFilter, Paging paging, Sorting sorting);

        Task<CatShelter?> GetCatShelterByIdAsync(Guid id);

        Task<bool> InsertCatShelterAsync(CatShelterAddModel catShelterAddModel);

        Task<bool> UpdateCatShelterByIdAsync(Guid id, CatShelterUpdateModel catShelterUpdateModel);

        Task<bool> DeleteCatShelterByIdAsync(Guid id);
    }
}
