using AuchanTest.Models;
using AuchanTest.Models.Filters;

namespace AuchanTest.Services
{
    public interface ICarsService
    {
        CarSearchResult Search(CarFilters filters);
    }
}