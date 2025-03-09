using AuchanTest.Infrastructure.Converters;
using AuchanTest.Models;
using CsvHelper.Configuration;

namespace AuchanTest.Infrastructure.Mappers
{
    public class CarMapper : ClassMap<Car>
    {
        public CarMapper()
        {
            Map(m => m.Brand).Name("Brand");
            Map(m => m.Model).Name("Model");
            Map(m => m.Year).Name("Year");
            Map(m => m.EngineSize).Name("Engine_Size");
            Map(m => m.FuelType).Name("Fuel_Type");
            Map(m => m.Transmission).Name("Transmission").TypeConverter<CaseInsensitiveEnumConverter<TransmisionEnum>>();
            Map(m => m.Mileage).Name("Mileage");
            Map(m => m.Doors).Name("Doors");
            Map(m => m.OwnerCount).Name("Owner_Count");
            Map(m => m.Price).Name("Price");
        }
    }
}
