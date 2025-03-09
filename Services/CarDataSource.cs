using AuchanTest.Infrastructure.Mappers;
using AuchanTest.Models;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace AuchanTest.Services
{
    public sealed class CarDataSource : ICarDataSource
    {
        private const string DatasetFilePath = "car_price_dataset.csv";
        private readonly ILogger<CarDataSource> _logger;

        public CarDataSource(ILogger<CarDataSource> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IReadOnlyList<Car> LoadCars()
        {
            try
            {
                using var reader = new StreamReader(DatasetFilePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.Context.RegisterClassMap<CarMapper>();
                return csv.GetRecords<Car>()?.ToList() ?? new List<Car>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load car dataset from {FilePath}", DatasetFilePath);
                return Array.Empty<Car>();
            }
        }
    }
}
