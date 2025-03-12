# Blazor Server CRUD Application

A Blazor Server application demonstrating CRUD operations with a weather forecast service.

## Project Structure

- `BlazorServerCRUD.Web/` - Main web application
- `BlazorServerCRUD.Tests/` - Unit tests for the application

## Prerequisites

- .NET 7.0 SDK or later
- Visual Studio Code, Visual Studio, or any preferred IDE

## Development Setup

1. Clone the repository:
```bash
git clone <repository-url>
cd blazor-test
```

2. Build the solution:
```bash
dotnet build
```

3. Run the tests:
```bash
dotnet test
```

4. Run the application:
```bash
dotnet watch run --project BlazorServerCRUD.Web
```

5. Open your browser and navigate to:
- Local development: `http://localhost:5000`
- Dev Container: `http://localhost:5000`

## Development in Dev Container

If you're using the Dev Container:

1. Open the project in VS Code
2. When prompted, click "Reopen in Container"
3. Wait for the container to build
4. Run the application using:
```bash
dotnet watch run --project BlazorServerCRUD.Web
```

## Running Tests

```bash
dotnet test BlazorServerCRUD.Tests/BlazorServerCRUD.Tests.csproj
```

## Project Features

- Weather forecast service demonstration
- Server-side Blazor implementation
- Unit tests for core functionality

## Contributing

1. Create a new branch for your feature
2. Make your changes
3. Run the tests to ensure everything works
4. Submit a pull request

## License

[Add your license information here]
