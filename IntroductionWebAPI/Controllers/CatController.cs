using Introduction.Model;
using Introduction.Service.Common;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IntroductionWebAPI.Controllers
{
    [ApiController]
    [Route("cats")]
    public class CatController : Controller
    {
        private readonly ICatService _catService;

        public CatController(ICatService catService)
        {
            _catService = catService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatsAsync(string name = "", int? age = null, string color = "",
            DateOnly? arrivalDateAfter = null, DateOnly? arrivalDateBefore = null)
        {
            List<Cat>? cats = await _catService.GetCatsAsync(name, age, color, arrivalDateAfter, arrivalDateBefore);
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
            Cat? cat = await _catService.GetCatAsync(id);
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
            bool isAdded = await _catService.PostCatAsync(catAddModel);
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
            bool isUpdated = await _catService.PutCatAsync(id, catUpdateModel);
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
            bool isDeleted = await _catService.DeleteCatAsync(id);
            if (!isDeleted)
            {
                return BadRequest("Cat has not been deleted.");
            }
            return NoContent();
        }
    }
}
