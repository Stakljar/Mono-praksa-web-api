using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IntroductionWebAPI.Controllers
{
    [ApiController]
    [Route("cats")]
    public class CatController : Controller
    {
        private static readonly List<Cat> cats = [
            new Cat(1, "Brooks", 3, "Mixed", new DateOnly(2023, 11, 20)),
            new Cat(2, "Blacky", 2, "Black", new DateOnly(2024, 1, 14)),
        ];

        [HttpGet]
        public IActionResult GetCats(int? age = null, string color = "", DateOnly? arrivalDateAfter = null, DateOnly? arrivalDateBefore = null)
        {
            IEnumerable<Cat> filteredCats = cats;

            filteredCats = filteredCats.Where(cat =>
                (string.IsNullOrEmpty(color) || cat.Color.Equals(color, StringComparison.OrdinalIgnoreCase)) &&
                (!age.HasValue || cat.Age == age.Value) &&
                (!arrivalDateAfter.HasValue || cat.ArrivalDate > arrivalDateAfter.Value) &&
                (!arrivalDateBefore.HasValue || cat.ArrivalDate < arrivalDateBefore.Value)
            );
            return Ok(filteredCats.ToList());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetCat(int id)
        {
            return Ok(cats.FirstOrDefault(cat => cat.Id == id));
        }

        [HttpPost]
        [Route("add")]
        public IActionResult PostCat([FromBody][Required] CatAddModel catAddModel)
        {
            long nextId = cats.Max(cat => cat.Id) + 1;
            cats.Add(new Cat(nextId, catAddModel.Name, catAddModel.Age, catAddModel.Color, catAddModel.ArrivalDate.Value));
            return StatusCode(201);
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult PutCat(long id, [FromBody][Required] CatUpdateModel catUpdateModel)
        {
            var existingCat = cats.FirstOrDefault(c => c.Id == id);
            if (existingCat == null)
            {
                return NotFound($"Cat with Id {id} not found.");
            }

            if (catUpdateModel.Age.HasValue)
            {
                existingCat.Age = catUpdateModel.Age.Value;
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteCat(long id)
        {
            var catToRemove = cats.FirstOrDefault(c => c.Id == id);
            if (catToRemove == null)
            {
                return NotFound($"Cat with Id {id} not found.");
            }
            cats.Remove(catToRemove);
            return NoContent();
        }
    }
}
