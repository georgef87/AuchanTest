using System;

namespace AuchanTest.Models.Filters
{
    public class PagingParams
    {
        private const int MinPageSize = 1;
        private const int DefaultPageSize = 5;
        private int _pageSize;

        public PagingParams()
        {
            Page = 1;
            PageSize = DefaultPageSize;
        }

        public int Page { get; set; }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value >= MinPageSize ? value : throw new ArgumentException($"PageSize must be at least {MinPageSize}.");
        }
    }
}
