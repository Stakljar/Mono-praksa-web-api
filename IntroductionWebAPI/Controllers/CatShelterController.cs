using Introduction.Model;
using Introduction.Service.Common;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IntroductionWebAPI.Controllers
{
    [ApiController]
    [Route("cat_shelters")]
    public class CatShelterController : Controller
    {
        private readonly ICatShelterService _catShelterService;

        public CatShelterController(ICatShelterService catShelterService)
        {
            _catShelterService = catShelterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatSheltersAsync(string name = "", string location = "",
            DateOnly? createdAtDateAfter = null, DateOnly? createdAtDateBefore = null)
        {
            List<CatShelter>? catsShelters = await _catShelterService.GetCatSheltersAsync(name, location, createdAtDateAfter, createdAtDateBefore);
            if (catsShelters == null)
            {
                return BadRequest("Returned null value.");
            }
            return Ok(catsShelters);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCatShelterAsync(Guid id)
        {
            CatShelter? catShelter = await _catShelterService.GetCatShelterAsync(id);
            if (catShelter == null)
            {
                return BadRequest("Returned null value.");
            }
            return Ok(catShelter);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> PostCatShelterAsync([FromBody][Required] CatShelterAddModel catShelterAddModel)
        {
            bool isAdded = await _catShelterService.PostCatShelterAsync(catShelterAddModel);
            if (!isAdded)
            {
                return BadRequest("Cat shelter has not been inserted.");
            }
            return StatusCode(201);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> PutCatShelterAsync(Guid id, [FromBody][Required] CatShelterUpdateModel catShelterUpdateModel)
        {
            bool isUpdated = await _catShelterService.PutCatShelterAsync(id, catShelterUpdateModel);
            if (!isUpdated)
            {
                return BadRequest("Cat shelter has not been updated.");
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteCatShelterAsync(Guid id)
        {
            bool isUpdated = await _catShelterService.DeleteCatShelterAsync(id);
            if (!isUpdated)
            {
                return BadRequest("Cat shelter has not been deleted.");
            }
            return NoContent();
        }
    }
}
