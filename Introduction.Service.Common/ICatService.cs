
using Introduction.Model;

namespace Introduction.Service.Common
{
    public interface ICatService
    {
        List<Cat>? GetCats(string name = "", int? age = null, string color = "",
            DateOnly? ArrivalDateAfter = null, DateOnly? ArrivalDateBefore = null);

        Cat? GetCat(Guid id);

        bool PostCat(CatAddModel catAddModel);

        bool PutCat(Guid id, CatUpdateModel catUpdateModel);

        bool DeleteCat(Guid id);
    }
}
