using Introduction.Common;
using Introduction.Model;
using Introduction.Service.Common;
using Introduction.WebAPI.RestModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Introduction.WebAPI.Controllers
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
        public async Task<IActionResult> GetCatSheltersAsync(string catShelterName = "", string catShelterLocation = "",
            DateOnly? catShelterEstablishedAtDateAfter = null, DateOnly? catShelterEstablishedAtDateBefore = null, string catName = "", int? catAgeAbove = null,
            int? catAgeBelow = null, string catColor = "", DateOnly? catArrivalDateAfter = null, DateOnly? catArrivalDateBefore = null,
            int pageSize = 10, int pageNumber = 1, string sortBy = "Id", bool isAscending = false)
        {

            CatShelterFilter catShelterFilter = new()
            {
                Name = catShelterName,
                Location = catShelterLocation,
                EstablishedAtAfter = catShelterEstablishedAtDateAfter,
                EstablishedAtBefore = catShelterEstablishedAtDateBefore,
            };

            CatFilter catFilter = new()
            {
                Name = catName,
                AgeAbove = catAgeAbove,
                AgeBelow = catAgeBelow,
                Color = catColor,
                ArrivalDateAfter = catArrivalDateAfter,
                ArrivalDateBefore = catArrivalDateBefore,
            };

            Paging paging = new()
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
            };

            Sorting sorting = new()
            {
                SortBy = sortBy,
                IsAscending = isAscending,
            };

            List<CatShelter>? catsShelters = await _catShelterService.GetCatSheltersAsync(catShelterFilter, catFilter, paging, sorting);
            if (catsShelters == null)
            {
                return BadRequest("Returned null value.");
            }
            List<CatShelterGetModel> catsShelterGetModels = catsShelters
                .Select(cs => new CatShelterGetModel
                {
                    Id = cs.Id,
                    Name = cs.Name,
                    Location = cs.Location,
                    EstablishedAt = (DateOnly)cs.EstablishedAt,
                    Cats = cs.Cats.Select(c => new CatGetModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Age = (int)c.Age,
                        Color = c.Color,
                        ArrivalDate = c.ArrivalDate,
                        CatShelterId = c.CatShelterId,
                    }).ToList()
                })
                .ToList();
            return Ok(catsShelterGetModels);
        }

        [HttpGet("without_cats")]
        public async Task<IActionResult> GetCatSheltersWithoutCatsAsync()
        {
            List<CatShelter>? catsSheltersWithoutCats = await _catShelterService.GetCatSheltersWithoutCatsAsync();
            if (catsSheltersWithoutCats == null)
            {
                return BadRequest("Returned null value.");
            }
            List<CatShelterWithoutCatsGetModel> catShelterWithoutCatsGetModel = catsSheltersWithoutCats
                .Select(cs => new CatShelterWithoutCatsGetModel
                {
                    Id = cs.Id,
                    Name = cs.Name,
                    Location = cs.Location,
                    EstablishedAt = (DateOnly)cs.EstablishedAt,
                })
                .ToList();
            return Ok(catShelterWithoutCatsGetModel);
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
            CatShelterGetModel catShelterGetModel = new()
            {
                Id = catShelter.Id,
                Name = catShelter.Name,
                Location = catShelter.Location,
                EstablishedAt = (DateOnly)catShelter.EstablishedAt,
                Cats = catShelter.Cats.Select(c => new CatGetModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Age = (int)c.Age,
                    Color = c.Color,
                    ArrivalDate = c.ArrivalDate,
                    CatShelterId = c.CatShelterId,
                }).ToList()
            };
            return Ok(catShelterGetModel);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> PostCatShelterAsync([FromBody][Required] CatShelterAddModel catShelterAddModel)
        {
            CatShelter catShelter = new()
            {
                Name = catShelterAddModel.Name,
                Location = catShelterAddModel.Location,
                EstablishedAt = catShelterAddModel.EstablishedAt,
            };

            bool isAdded = await _catShelterService.PostCatShelterAsync(catShelter);
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
            CatShelter catShelter = new()
            {
                Id = id,
                Name = catShelterUpdateModel.Name,
                Location = catShelterUpdateModel.Location,
                EstablishedAt = catShelterUpdateModel.EstablishedAt,
            };

            bool isUpdated = await _catShelterService.PutCatShelterAsync(catShelter);
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
