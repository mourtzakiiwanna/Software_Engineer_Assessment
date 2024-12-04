# Software Engineer Assessment

## Description

This project implements a Web API to solve the following assessment questions:

1. **Find the Second Largest Integer**: An HTTP POST endpoint that receives a JSON body containing an array of integers and returns the second largest integer.
2. **Retrieve Country Information**: An HTTP GET endpoint that calls the [REST Countries API](https://restcountries.com/#endpoints-currency), retrieves data about countries, and returns specific properties.
   - **Note:** I had to use the "/currency" endpoint instead of the "/all" endpoint, because the last one was giving me some time-out exceptions due to data volume
4. **Store Country Data**: Saves the retrieved country data into a Microsoft SQL Server database.
5. **Caching Mechanism**: Implements caching using `MemoryCache` to optimize data retrieval, checking the cache first before querying the database or making an external API call.

## Features

- **POST /second-largest**: Accepts a JSON body with an array of integers and returns the second largest integer.
- **GET /countries**: Retrieves country information, caching results to reduce API calls and database queries.
- **Database Integration**: Utilizes Entity Framework Core to interact with a SQL Server database.
- **In-Memory Caching**: Uses `MemoryCache` to store and retrieve country data efficiently.

## Technologies Used

- C#
- ASP.NET Core
- Entity Framework Core
- SQL Server
- In-Memory Cache
- Newtonsoft.Json (for JSON handling)
- Moq (for unit testing)

## Getting Started

### Prerequisites

Ensure you have the following installed on your machine:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 preferred)
- SQL Server (local instance or a connection string to a remote instance)
- A code editor (like Visual Studio or Visual Studio Code)

### Installation

1. Clone the repository to your local machine
2. Restore the dependencies
```bash
dotnet restore
```

3. Update your connection string for SQL Server in appsettings.json:

"ConnectionStrings": {
    "CountryDB": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Integrated Security=True;Trusted_Connection=True;"
}

### Running the Application
To run the application, use the following command in the terminal:

```bash
dotnet run
```

### API Endpoints
## POST /Array
Request Body:
```json
{
    "requestArrayObj": [5, 1, 3, 4, 2]
}
```
```bash
Response: 4
```

## GET /countries
_**Note:** I had to use the "/currency" endpoint instead of the "/all" endpoint, because the last one was giving me some time-out exceptions due to data volume_

Response: Returns a list of countries with the following properties:
- Common Name
- Capital
- Borders

### Running Tests
To run the unit tests, navigate to the test project directory and execute:
```bash
dotnet test
```

This will run all the unit tests and provide feedback on their success or failure.
