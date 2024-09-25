using Introduction.Common;
using Introduction.Model;

namespace Introduction.Service.Common
{
    public interface ICatShelterService
    {
        Task<List<CatShelter>?> GetCatSheltersAsync(CatShelterFilter catShelterFilter, CatFilter catFilter, Paging paging, Sorting sorting);

        Task<List<CatShelter>?> GetCatSheltersWithoutCatsAsync();

        Task<CatShelter?> GetCatShelterAsync(Guid id);

        Task<bool> PostCatShelterAsync(CatShelter catShelter);

        Task<bool> PutCatShelterAsync(CatShelter catShelter);

        Task<bool> DeleteCatShelterAsync(Guid id);
    }
}
