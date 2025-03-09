# AuchanTest

## Overview

AuchanTest is a .NET Core application designed in the scope of the test

## Requirements

- .NET 8.0 SDK for building and running outside of a container
- Docker for containerization build and run 

## Getting Started

### Building the Project

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/AuchanTest.git
    cd AuchanTest
    ```

2. Build the project:
    ```sh
    dotnet build
    ```

### Running the Project

1. Run the project:
    ```sh
    dotnet run
    ```

2. The application will be available at `https://localhost:51449`.

### Running with Docker

1. Build the Docker image:
    ```sh
    docker build -t auchantest -f Dockerfile .
    ```

2. Run the Docker container:
    ```sh
    docker run -d -p 8080:8080 --name auchantest auchantest
    ```

3. The application will be available at `https://localhost:8080`.

## API Endpoints

### Search Cars

- **GET** `/api/cars/search`
    - Query Parameters:
        - `brand` (string): Car brand
        - `minYear` (int): Minimum year
        - `maxMileage` (int): Maximum mileage
        - `fuelType` (string): Fuel type
        - `minPrice` (int): Minimum price
        - `maxPrice` (int): Maximum price
        - `sortBy` (string): Sort by criteria
        - `page` (int): Page number
        - `pageSize` (int): Page size
    
#### Example cURL Request

```sh
curl -k -X GET "https://localhost:51449/api/cars/search?brand=ford&minYear=2018&maxPrice=25000&sortBy=year_desc&page=1&pageSize=10"
```
```sh
{
  "results": [
    {
      "brand": "Ford",
      "model": "Explorer",
      "year": 2023,
      "engine_Size": 4.1,
      "fuelType": "Electric",
      "transmission": "Automatic",
      "mileage": 67072,
      "doors": 4,
      "ownerCount": 3,
      "price": 17258
    },
    ...
  ],
  "summary": {
    "averagePrice": 11468.2339622642,
    "mostCommonFuelType": "Electric",
    "newestCarYear": 2023
  },
  "pagination": {
    "totalCount": 265,
    "page": 1,
    "pageSize": 10
  }
}
```