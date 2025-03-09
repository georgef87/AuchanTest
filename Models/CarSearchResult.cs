using System.Collections.Generic;

namespace AuchanTest.Models
{
    public class CarSearchResult
    {
        public IEnumerable<Car> Results { get; set; }
        public Summary Summary { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class Summary
    {
        public double AveragePrice { get; set; }
        public FuelTypeEnum MostCommonFuelType { get; set; }
        public int NewestCarYear { get; set; }
    }

    public class Pagination
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}