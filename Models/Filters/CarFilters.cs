using System;

namespace AuchanTest.Models.Filters
{
    public class CarFilters
    {
        public CarFilters(FilterParams filterParams, string sortBy, PagingParams pagingParams)
        {
            FilterParams = filterParams ?? throw new ArgumentNullException(nameof(filterParams));
            PagingParams = pagingParams ?? throw new ArgumentNullException(nameof(pagingParams));
            SortBy = sortBy;
        }

        public FilterParams FilterParams { get; }
        public PagingParams PagingParams { get; }
        public string SortBy { get; }
    }
}
