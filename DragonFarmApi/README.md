# Dragon Farm API

A comprehensive REST API for managing a dragon farm, built with ASP.NET Core, Entity Framework Core, and SQL Server.

## Features

- **Dragon Management**: Full CRUD operations for dragons
- **Feeding Records**: Track feeding history for each dragon
- **Swagger Documentation**: Interactive API documentation
- **SQL Server Integration**: Persistent data storage
- **Data Validation**: Input validation with detailed error messages

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB is configured by default)

### Running the Application

1. **Clone and navigate to the project**
   ```bash
   cd DragonFarmApi
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access the API**
   - API Base URL: `https://localhost:7XXX` (port will be shown in console)
   - Swagger UI: Navigate to the base URL in your browser (e.g., `https://localhost:7XXX`)

### Database

The application uses SQL Server LocalDB by default. The database will be created automatically when you first run the application. The connection string can be modified in `appsettings.json`.

## API Endpoints

### Dragons

- `GET /api/dragons` - Get all dragons
- `GET /api/dragons/{id}` - Get a specific dragon
- `POST /api/dragons` - Create a new dragon
- `PUT /api/dragons/{id}` - Update an existing dragon
- `DELETE /api/dragons/{id}` - Delete a dragon

### Feeding Records

- `GET /api/feedingrecords` - Get all feeding records
- `GET /api/feedingrecords/{id}` - Get a specific feeding record
- `GET /api/feedingrecords/dragon/{dragonId}` - Get feeding records for a specific dragon
- `POST /api/feedingrecords` - Create a new feeding record
- `DELETE /api/feedingrecords/{id}` - Delete a feeding record

## Sample Data

The application includes sample dragons:
- **Smaug**: A fierce fire-breathing dragon (Red Fire Dragon)
- **Frostbite**: A majestic ice dragon from the northern mountains (Blue Ice Dragon)

## Technology Stack

- **ASP.NET Core 9.0**: Web framework
- **Entity Framework Core**: ORM for database operations
- **SQL Server**: Database
- **Swagger/OpenAPI**: API documentation
- **Data Annotations**: Input validation

## Configuration

### Connection String

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DragonFarmDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### CORS

CORS is configured to allow all origins for development. Modify the CORS policy in `Program.cs` for production use.

## Development

The project structure:

```
DragonFarmApi/
??? Controllers/           # API controllers
??? Models/               # Entity models
??? DTOs/                 # Data Transfer Objects
??? Data/                 # Database context
??? Program.cs            # Application entry point
??? appsettings.json      # Configuration
??? DragonFarmApi.csproj  # Project file
```

Enjoy managing your dragon farm! ??