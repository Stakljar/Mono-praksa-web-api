using Introduction.Model;

namespace Introduction.Repository.Common
{
    public interface ICatRepository
    {
        List<Cat>? GetCats(string name = "", int? age = null, string color = "",
            DateOnly? ArrivalDateAfter = null, DateOnly? ArrivalDateBefore = null);

        Cat? GetCatById(Guid id);

        bool InsertCat(CatAddModel catAddModel);

        bool UpdateCatById(Guid id, CatUpdateModel catUpdateModel);

        bool DeleteCatById(Guid id);
    }
}
