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
        public async Task<IActionResult> GetCatsAsync(string name = "", int? age = null, string color = "",
            DateOnly? ArrivalDateAfter = null, DateOnly? ArrivalDateBefore = null)
        {
            List<Cat>? cats = await catService.GetCatsAsync(name, age, color, ArrivalDateAfter, ArrivalDateBefore);
            if(cats == null)
            {
                return BadRequest("Returned null value.");
            }
            return Ok(cats);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCatAsync(Guid id)
        {
            Cat? cat = await catService.GetCatAsync(id);
            if (cat == null)
            {
                return BadRequest("Returned null value.");
            }
            return Ok(cat);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> PostCatAsync([FromBody][Required] CatAddModel catAddModel)
        {
            bool isAdded = await catService.PostCatAsync(catAddModel);
            if(!isAdded)
            {
                return BadRequest("Cat has not been inserted.");
            }
            return StatusCode(201);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> PutCatAsync(Guid id, [FromBody][Required] CatUpdateModel catUpdateModel)
        {
            bool isUpdated = await catService.PutCatAsync(id, catUpdateModel);
            if (!isUpdated)
            {
                return BadRequest("Cat has not been updated.");
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteCatAsync(Guid id)
        {
            bool isDeleted = await catService.DeleteCatAsync(id);
            if (!isDeleted)
            {
                return BadRequest("Cat has not been deleted.");
            }
            return NoContent();
        }
    }
}
