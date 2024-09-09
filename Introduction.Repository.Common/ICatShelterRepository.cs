
using Introduction.Model;

namespace Introduction.Repository.Common
{
    public interface ICatShelterRepository
    {
        Task<List<CatShelter>?> GetCatSheltersAsync(string name = "", string location = "",
            DateOnly? createdAtDateAfter = null, DateOnly? createdAtDateBefore = null);

        Task<CatShelter?> GetCatShelterByIdAsync(Guid id);

        Task<bool> InsertCatShelterAsync(CatShelterAddModel catShelterAddModel);

        Task<bool> UpdateCatShelterByIdAsync(Guid id, CatShelterUpdateModel catShelterUpdateModel);

        Task<bool> DeleteCatShelterByIdAsync(Guid id);
    }
}
