using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuchanTest.Services;
using AuchanTest.Infrastructure.Exceptions;
using AuchanTest.Models.Filters;

namespace AuchanTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private ICarsService _carsService;

        public CarsController(ILogger<CarsController> logger, ICarsService carsService)
        {
            _carsService = carsService;

        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string brand,
                [FromQuery] int? minYear,
                [FromQuery] int? maxMileage,
                [FromQuery] string fuelType,
                [FromQuery] int? minPrice,
                [FromQuery] int? maxPrice,
                [FromQuery] string sortBy,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10)
        {

            var filterParams = new FilterParams(brand, minYear, maxMileage, fuelType, minPrice, maxPrice);
            var pagingParams = new PagingParams() { Page = page, PageSize = pageSize };
            var filter = new CarFilters(filterParams, sortBy, pagingParams);

            try
            {
                var ret = _carsService.Search(filter);
                return Ok(ret);
            }
            catch (InvalidFilterException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}