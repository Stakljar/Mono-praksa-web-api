
using Introduction.Model;

namespace Introduction.Repository.Common
{
    public interface ICatShelterRepository
    {
        List<CatShelter>? GetCatShelters(string name = "", string location = "", DateOnly? createdAtDateAfter = null, DateOnly? createdAtDateBefore = null);

        CatShelter? GetCatShelterById(Guid id);

        bool InsertCatShelter(CatShelterAddModel catShelterAddModel);

        bool UpdateCatShelterById(Guid id, CatShelterUpdateModel catShelterUpdateModel);

        bool DeleteCatShelterById(Guid id);
    }
}
