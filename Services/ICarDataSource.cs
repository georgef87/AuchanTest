using AuchanTest.Models;
using System.Collections.Generic;

namespace AuchanTest.Services
{
    public interface ICarDataSource
    {
        public IReadOnlyList<Car> LoadCars();

    }
}
