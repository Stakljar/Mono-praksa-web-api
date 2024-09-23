using Introduction.Common;
using Introduction.Model;
using Introduction.Service.Common;
using Introduction.WebAPI.RestModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Introduction.WebAPI.Controllers
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
        public async Task<IActionResult> GetCatsAsync(string name = "", int? ageAbove = null, int? ageBelow = null, string color = "",
            DateOnly? arrivalDateAfter = null, DateOnly? arrivalDateBefore = null, int pageSize = 10, int pageNumber = 1, string sortBy = "Id",
            bool isAscending = false)
        {
            CatFilter catFilter = new()
            {
                Name = name,
                AgeAbove = ageAbove,
                AgeBelow = ageBelow,
                Color = color,
                ArrivalDateAfter = arrivalDateAfter,
                ArrivalDateBefore = arrivalDateBefore
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

            List<Cat>? cats = await _catService.GetCatsAsync(catFilter, paging, sorting);
            if(cats == null)
            {
                return BadRequest("Returned null value.");
            }
            List<CatGetModel> catGetModels = cats
                .Select(c => new CatGetModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Age = (int)c.Age,
                    Color = c.Color,
                    ArrivalDate = c.ArrivalDate,
                    CatShelterId = c.CatShelterId,
                }).ToList();
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
            CatGetModel catGetModel = new()
            {
                Id = cat.Id,
                Name = cat.Name,
                Age = (int)cat.Age,
                Color = cat.Color,
                ArrivalDate = cat.ArrivalDate,
                CatShelterId = cat.CatShelterId,
            };
            return Ok(cat);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> PostCatAsync([FromBody][Required] CatAddModel catAddModel)
        {
            Cat cat = new()
            {
                Name = catAddModel.Name,
                Age = catAddModel.Age,
                Color = catAddModel.Color,
                ArrivalDate = catAddModel.ArrivalDate,
            };

            bool isAdded = await _catService.PostCatAsync(cat);
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
            Cat cat = new()
            {
                Id = id,
                Age = catUpdateModel.Age,
                Color = catUpdateModel.Color,
                ArrivalDate = catUpdateModel.ArrivalDate,
            };

            bool isUpdated = await _catService.PutCatAsync(cat);
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
