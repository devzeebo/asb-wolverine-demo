# Wolverine Demo

This project demonstrates Wolverine over Azure Service Bus.

## What is this?

This is a **demo application** showcasing how to use the Wolverine framework with:

- **ASP.NET**: HTTP endpoints
- **Wolverine Framework**: Message handling and workflow orchestration
- **Entity Framework Core**: Data access with SQL Server, Wolverine's outbox and inbox, and Wolverine's durable queues
- **Azure Service Bus**: Message queuing and pub/sub

The demo is designed to help developers understand how to integrate these technologies together in a .NET 8 application.

## Local Development Setup

### Prerequisites

- Docker and Docker Compose installed
- .NET 8 SDK
- Azure CLI installed and configured (`az login`)

### Starting Local Infrastructure

1. Start the local services:

```bash
docker-compose up -d
```

This will start:

- **SQL Server** on port 1433

  - Username: `sa`
  - Password: `Password123!`
  - Database: `WolverineDemo` (will be created automatically)

### Azure Identity Configuration

This project uses Azure Identity for Service Bus authentication instead of connection strings. This provides:

- **Secure authentication**: No hardcoded connection strings or keys
- **Environment flexibility**: Works with local development and Azure
- **Modern Azure practices**: Uses token-based authentication

#### Local Development Setup

1. **Install Azure CLI**: Download and install from [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

2. **Login to Azure**:

```bash
az login
```

3. **Set subscription** (if you have multiple):

```bash
az account set --subscription "Your Subscription Name"
```

The application will automatically use `AzureCliCredential` in development mode, which reads from your Azure CLI login.

### Running the Application

1. **Navigate to the API project**:

```bash
cd WolverineDemo.Api
```

2. **Update the Service Bus FQDN** in user secrets:

```jsonc
{
  "Wolverine": {
    "ServiceBus": {
      "FQDN": "your-namespace.servicebus.windows.net"
    }
  }
}
```

3. **Run the application**:

```bash
dotnet run
```

Or using the .NET CLI:

```bash
dotnet watch run
```

The API will be available at:

- **API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger

### Running Tests

1. **Navigate to the solution root**:

```bash
cd /path/to/wolverine-demo
```

2. **Run all tests**:

```bash
dotnet test
```

3. **Run tests with specific project**:

```bash
dotnet test WolverineDemo.Tests
```

4. **Run tests with watch mode**:

```bash
dotnet watch test
```

### Building the Project

1. **Build the entire solution**:

```bash
dotnet build
```

2. **Build specific project**:

```bash
dotnet build WolverineDemo.Api
```

3. **Build in Release mode**:

```bash
dotnet build --configuration Release
```

### Stopping Services

```bash
docker-compose down
```

To remove volumes (this will delete all data):

```bash
docker-compose down -v
```

### Health Checks

The services include health checks to ensure they're ready:

- SQL Server: Checks if the database is responding

### Connection Details

The application is configured to connect to these local services via the `appsettings.json` file:

- **SQL Server**: `Server=127.0.0.1,1433;Database=WolverineDemo;User Id=sa;Password=Password123!;TrustServerCertificate=true`
- **Azure Service Bus**: Configure the FQDN in the `Wolverine:ServiceBus:FQDN` setting

### Database Initialization

The SQL Server container will start with an empty database. You may need to run your application's database initialization/migration scripts to create the required tables and schema.

### Troubleshooting

If you encounter connection issues:

1. Ensure Docker is running
2. Check if containers are healthy: `docker-compose ps`
3. View logs: `docker-compose logs [service-name]`
4. Restart services: `docker-compose restart`
5. Verify Azure CLI login: `az account show`
6. Check if the API is running: `curl http://localhost:5000/swagger`
