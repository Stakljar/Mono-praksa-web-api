using Introduction.Common;
using Introduction.Model;

namespace Introduction.Repository.Common
{
    public interface ICatRepository
    {
        Task<List<Cat>?> GetCatsAsync(CatFilter catFilter, Paging paging, Sorting sorting);

        Task<Cat?> GetCatByIdAsync(Guid id);

        Task<bool> InsertCatAsync(CatAddModel catAddModel);

        Task<bool> UpdateCatByIdAsync(Guid id, CatUpdateModel catUpdateModel);

        Task<bool> DeleteCatByIdAsync(Guid id);
    }
}
