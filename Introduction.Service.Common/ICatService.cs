using Introduction.Common;
using Introduction.Model;

namespace Introduction.Service.Common
{
    public interface ICatService
    {
        Task<List<Cat>?> GetCatsAsync(CatFilter catFilter, Paging paging, Sorting sorting);

        Task<Cat?> GetCatAsync(Guid id);

        Task<bool> PostCatAsync(CatAddModel catAddModel);

        Task<bool> PutCatAsync(Guid id, CatUpdateModel catUpdateModel);

        Task<bool> DeleteCatAsync(Guid id);
    }
}
