# Spaceport

Spaceport is the central API service of the Cortex system, designed to fetch data from third-party APIs and share it with other microservices.

## Project Structure

```
Spaceport/
├── src/                         # Source code
│   ├── Spaceport.API/           # API controllers and endpoints
│   ├── Spaceport.Domain/        # Domain models and interfaces
│   ├── Spaceport.Infrastructure/ # Implementation of domain interfaces
│   └── Spaceport.Host/          # Application host and entry point
├── tests/                       # Test projects
│   ├── Spaceport.UnitTests/     # Unit tests
│   └── Spaceport.IntegrationTests/ # Integration tests
├── infrastructure/              # Docker and Kubernetes configurations
└── Spaceport.sln                # Solution file
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- Docker and Docker Compose (for containerized development)

### Development

1. Clone the repository

2. Build the solution
```
dotnet build
```

3. Run the API
```
dotnet run --project src/Spaceport.Host/Spaceport.Host.csproj
```

### Using Docker

1. Build and run using Docker Compose
```
cd infrastructure
docker-compose up --build
```

## Features

- Context switching for work items
- Fetch and aggregate data from Azure DevOps
- Organize and manage work items, branches, PRs, and notes
- Track events and actions for work items

## API Endpoints

- `GET /api/context` - Get all contexts
- `GET /api/context/{id}` - Get a specific context
- `POST /api/context` - Create a new context
- `PUT /api/context/{id}` - Update a context
- `DELETE /api/context/{id}` - Delete a context
- `POST /api/context/{id}/switch` - Switch to a specific context
- `GET /api/context/current` - Get the current active context