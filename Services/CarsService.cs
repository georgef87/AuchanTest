using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.Extensions.Logging;
using AuchanTest.Models;
using System;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using AuchanTest.Infrastructure.Mappers;
using AuchanTest.Models.Filters;
using AuchanTest.Infrastructure.Exceptions;

namespace AuchanTest.Services
{
    public class CarsService : ICarsService
    {
        private readonly ILogger<CarsService> _logger;
        private readonly IMemoryCache _cache;
        private readonly IReadOnlyList<Car> Cars;

        public CarsService(
            ICarDataSource carDataSource,
            ILogger<CarsService> logger,
            IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
            Cars = carDataSource.LoadCars();
        }

        public CarSearchResult Search(CarFilters filters)
        {
            if (filters != null && !filters.FilterParams.IsValid())
            {
                throw new InvalidFilterException();
            }

            IQueryable<Car> filteredCars = Cars.AsQueryable();
            // Generate a cache key based on the search and sort parameters
            var cacheKey = GenerateCacheKey(filters);

            if (filters != null)
            {
                // Check if the results are already cached            
                if (!_cache.TryGetValue(cacheKey, out filteredCars))
                {
                    filteredCars = Cars.AsQueryable();
                    if (filters.FilterParams != null)
                    {
                        filteredCars = ApplyFilters(filters, filteredCars);
                    }

                    filteredCars = ApplySorting(filters, filteredCars);
                }
            }

            CarSearchResult response = BuildResult(filters, filteredCars);

            _cache.Set(cacheKey, filteredCars, TimeSpan.FromMinutes(10));

            return response;
        }

        private static CarSearchResult BuildResult(CarFilters filters, IQueryable<Car> filteredCars)
        {
            var totalCount = filteredCars.Count();
            var carsPage = filteredCars.Skip((filters.PagingParams.Page - 1) * filters.PagingParams.PageSize).Take(filters.PagingParams.PageSize).ToList();

            // Calculate average price, most common fuel type, and newest car year
            var averagePrice = filteredCars.Average(c => c.Price);

            // Calculate the most common fuel type
            var fuelTypeCounts = filteredCars
                .GroupBy(c => c.FuelType)
                .ToDictionary(g => g.Key, g => g.Count());

            var mostCommonFuelType = fuelTypeCounts.OrderByDescending(kv => kv.Value).First().Key;
            var newestCarYear = filteredCars.Max(c => c.Year);

            var response = new CarSearchResult
            {
                Results = carsPage,
                Summary = new Summary
                {
                    AveragePrice = averagePrice,
                    MostCommonFuelType = mostCommonFuelType,
                    NewestCarYear = newestCarYear
                },
                Pagination = new Pagination
                {
                    TotalCount = totalCount,
                    Page = filters.PagingParams.Page,
                    PageSize = filters.PagingParams.PageSize
                }
            };
            return response;
        }

        private static IQueryable<Car> ApplySorting(CarFilters filters, IQueryable<Car> filteredCars)
        {
            var sortingOptions = new Dictionary<string, Func<IQueryable<Car>, IOrderedQueryable<Car>>>
                {
                    { "year_desc", q => q.OrderByDescending(c => c.Year) },
                    { "year_asc", q => q.OrderBy(c => c.Year) },
                    { "price_desc", q => q.OrderByDescending(c => c.Price) },
                    { "price_asc", q => q.OrderBy(c => c.Price) },
                    { "mileage_desc", q => q.OrderByDescending(c => c.Mileage) },
                    { "mileage_asc", q => q.OrderBy(c => c.Mileage) }
                };

            if (sortingOptions.TryGetValue(filters.SortBy?.ToLower() ?? "year_asc", out var sortFunc))
            {
                filteredCars = sortFunc(filteredCars);
            }
            else
            {
                filteredCars = filteredCars.OrderBy(c => c.Year);
            }

            return filteredCars;
        }

        private static IQueryable<Car> ApplyFilters(CarFilters filters, IQueryable<Car> filteredCars)
        {
            filteredCars = filteredCars
                .Where(c => string.IsNullOrEmpty(filters.FilterParams.Brand) || c.Brand.ToLower().Contains(filters.FilterParams.Brand.ToLower()))
                .Where(c => !filters.FilterParams.MinYear.HasValue || c.Year >= filters.FilterParams.MinYear.Value)
                .Where(c => !filters.FilterParams.MaxMileage.HasValue || c.Mileage <= filters.FilterParams.MaxMileage.Value)
                .Where(c => string.IsNullOrEmpty(filters.FilterParams.FuelType) || c.FuelType.ToString().ToLower() == filters.FilterParams.FuelType.ToLower())
                .Where(c => !filters.FilterParams.MinPrice.HasValue || c.Price >= filters.FilterParams.MinPrice.Value)
                .Where(c => !filters.FilterParams.MaxPrice.HasValue || c.Price <= filters.FilterParams.MaxPrice.Value);

            return filteredCars;
        }

        private string GenerateCacheKey(CarFilters filters)
        {
            var cacheKey = filters.SortBy;
            if (filters.FilterParams != null)
            {
                cacheKey = string.Join(cacheKey, $"{filters.FilterParams.Brand}_{filters.FilterParams.MinYear}_{filters.FilterParams.MaxMileage}_{filters.FilterParams.FuelType}_{filters.FilterParams.MinPrice}_{filters.FilterParams.MaxPrice}");
            }

            return cacheKey;
        }
    }
}