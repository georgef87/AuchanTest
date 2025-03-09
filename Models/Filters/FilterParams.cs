using System;
using System.Collections.Generic;
using System.Linq;

namespace AuchanTest.Models.Filters
{
    public class FilterParams
    {
        public FilterParams(string brand, int? minYear, int? maxMileage, string fuelType, int? minPrice, int? maxPrice)
        {
            Brand = brand?.Trim();
            MinYear = minYear;
            MaxMileage = maxMileage;
            FuelType = fuelType?.Trim();
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }

        public string Brand { get; }
        public int? MinYear { get; }
        public int? MaxMileage { get; }
        public string FuelType { get; }
        public int? MinPrice { get; }
        public int? MaxPrice { get; }

        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            if (MinYear.HasValue && MinYear < 0)
            {
                errors.Add("Minimum year cannot be negative.");
            }

            if (MaxMileage.HasValue && MaxMileage < 0)
            {
                errors.Add("Maximum mileage cannot be negative.");
            }

            if (MinPrice.HasValue && MinPrice < 0)
            {
                errors.Add("Minimum price cannot be negative.");
            }

            if (MaxPrice.HasValue && MaxPrice <= 0)
            {
                errors.Add("Maximum price must be greater than zero.");
            }

            if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice)
            {
                errors.Add("Minimum price must be less than or equal to maximum price.");
            }

            return errors.ToArray();
        }

        public bool IsValid()
        {
            return !Validate().Any();
        }
    }
}
