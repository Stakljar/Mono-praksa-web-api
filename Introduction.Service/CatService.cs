using Introduction.Model;
using Introduction.Repository;

namespace Introduction.Service
{
    public class CatService
    {
        private readonly CatRepository catRepository = new();

        public List<Cat>? GetCats(string name = "", int? age = null, string color = "",
            DateOnly? ArrivalDateAfter = null, DateOnly? ArrivalDateBefore = null)
        {
            return catRepository.GetCats(name, age, color, ArrivalDateAfter, ArrivalDateBefore);
        }

        public Cat? GetCat(Guid id)
        {
            return catRepository.GetCatById(id);
        }

        public bool PostCat(CatAddModel catAddModel)
        {
            return catRepository.InsertCat(catAddModel);
        }

        public bool PutCat(Guid id, CatUpdateModel catUpdateModel)
        {
            return catRepository.UpdateCatById(id, catUpdateModel);
        }

        public bool DeleteCat(Guid id)
        {
            return catRepository.DeleteCatById(id);
        }
    }
}
