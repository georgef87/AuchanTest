namespace AuchanTest.Models
{
    public enum FuelTypeEnum
    {
        Petrol = 1,
        Diesel,
        Electric,
        Hybrid
    }

    public enum TransmisionEnum
    {
        Manual = 1,
        Automatic,
        SemiAutomatic
    }

    public class Car
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double EngineSize { get; set; }
        public FuelTypeEnum FuelType { get; set; }
        public TransmisionEnum Transmission { get; set; }
        public int Mileage { get; set; }
        public int Doors { get; set; }
        public int OwnerCount { get; set; }
        public int Price { get; set; }
    }
}