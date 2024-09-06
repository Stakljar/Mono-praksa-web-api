using Introduction.Model;
using Introduction.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IntroductionWebAPI.Controllers
{
    [ApiController]
    [Route("cats")]
    public class CatController : Controller
    {
        private readonly CatService catService = new();

        [HttpGet]
        public IActionResult GetCats(string name = "", int? age = null, string color = "",
            DateOnly? ArrivalDateAfter = null, DateOnly? ArrivalDateBefore = null)
        {
            List<Cat>? cats = catService.GetCats(name, age, color, ArrivalDateAfter, ArrivalDateBefore);
            if(cats == null)
            {
                return BadRequest("Returned null value.");
            }
            return Ok(cats);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetCat(Guid id)
        {
            Cat? cat = catService.GetCat(id);
            if (cat == null)
            {
                return BadRequest("Returned null value.");
            }
            return Ok(cat);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult PostCat([FromBody][Required] CatAddModel catAddModel)
        {
            bool isAdded = catService.PostCat(catAddModel);
            if(!isAdded)
            {
                return BadRequest("Cat has not been inserted.");
            }
            return StatusCode(201);
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult PutCat(Guid id, [FromBody][Required] CatUpdateModel catUpdateModel)
        {
            bool isUpdated = catService.PutCat(id, catUpdateModel);
            if (!isUpdated)
            {
                return BadRequest("Cat has not been updated.");
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteCat(Guid id)
        {
            bool isDeleted = catService.DeleteCat(id);
            if (!isDeleted)
            {
                return BadRequest("Cat has not been deleted.");
            }
            return NoContent();
        }
    }
}
