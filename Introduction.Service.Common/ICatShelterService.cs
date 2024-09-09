
using Introduction.Model;

namespace Introduction.Service.Common
{
    public interface ICatShelterService
    {
        Task<List<CatShelter>?> GetCatSheltersAsync(string name = "", string location = "",
            DateOnly? createdAtDateAfter = null, DateOnly? createdAtDateBefore = null);

        Task<CatShelter?> GetCatShelterAsync(Guid id);

        Task<bool> PostCatShelterAsync(CatShelterAddModel catShelterAddModel);

        Task<bool> PutCatShelterAsync(Guid id, CatShelterUpdateModel catShelterUpdateModel);

        Task<bool> DeleteCatShelterAsync(Guid id);
    }
}
