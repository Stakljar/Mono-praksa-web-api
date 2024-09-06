
using Introduction.Model;

namespace Introduction.Service.Common
{
    public interface ICatShelterService
    {
        List<CatShelter>? GetCatShelters(string name = "", string location = "", DateOnly? createdAtDateAfter = null, DateOnly? createdAtDateBefore = null);

        CatShelter? GetCatShelter(Guid id);

        bool PostCatShelter(CatShelterAddModel catShelterAddModel);

        bool PutCatShelter(Guid id, CatShelterUpdateModel catShelterUpdateModel);

        bool DeleteCatShelter(Guid id);
    }
}
