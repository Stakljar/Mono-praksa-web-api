using Introduction.Common;
using Introduction.Model;

namespace Introduction.Service.Common
{
    public interface ICatShelterService
    {
        Task<List<CatShelter>?> GetCatSheltersAsync(CatShelterFilter catShelterFilter, CatFilter catFilter, Paging paging, Sorting sorting);

        Task<CatShelter?> GetCatShelterAsync(Guid id);

        Task<bool> PostCatShelterAsync(CatShelterAddModel catShelterAddModel);

        Task<bool> PutCatShelterAsync(Guid id, CatShelterUpdateModel catShelterUpdateModel);

        Task<bool> DeleteCatShelterAsync(Guid id);
    }
}
