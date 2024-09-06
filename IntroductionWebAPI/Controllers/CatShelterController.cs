using Introduction.Model;
using Introduction.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IntroductionWebAPI.Controllers
{
    [ApiController]
    [Route("cat_shelters")]
    public class CatShelterController : Controller
    {
        private readonly CatShelterService catShelterService = new();

        [HttpGet]
        public IActionResult GetCatShelters(string name = "", string location = "", DateOnly? createdAtDateAfter = null, DateOnly? createdAtDateBefore = null)
        {
            List<CatShelter>? catsShelters = catShelterService.GetCatShelters(name, location, createdAtDateAfter, createdAtDateBefore);
            if (catsShelters == null)
            {
                return BadRequest("Returned null value.");
            }
            return Ok(catsShelters);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetCatShelter(Guid id)
        {
            CatShelter? catShelter = catShelterService.GetCatShelter(id);
            if (catShelter == null)
            {
                return BadRequest("Returned null value.");
            }
            return Ok(catShelter);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult PostCatShelter([FromBody][Required] CatShelterAddModel catShelterAddModel)
        {
            bool isAdded = catShelterService.PostCatShelter(catShelterAddModel);
            if (!isAdded)
            {
                return BadRequest("Cat shelter has not been inserted.");
            }
            return StatusCode(201);
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult PutCatShelter(Guid id, [FromBody][Required] CatShelterUpdateModel catShelterUpdateModel)
        {
            bool isUpdated = catShelterService.PutCatShelter(id, catShelterUpdateModel);
            if (!isUpdated)
            {
                return BadRequest("Cat shelter has not been updated.");
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteCatShelter(Guid id)
        {
            bool isUpdated = catShelterService.DeleteCatShelter(id);
            if (!isUpdated)
            {
                return BadRequest("Cat shelter has not been deleted.");
            }
            return NoContent();
        }
    }
}
