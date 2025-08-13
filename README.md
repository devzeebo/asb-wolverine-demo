# Wolverine Demo

This project demonstrates Wolverine with local development infrastructure using Docker Compose and Azure Identity for Service Bus authentication.

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

- **Azure Service Bus Emulator** on port 44444

  - Uses the official Microsoft Azure Service Bus emulator container
  - Management endpoint available on port 5300

### Azure Identity Configuration

This project uses Azure Identity for Service Bus authentication instead of connection strings. This provides:

- **Secure authentication**: No hardcoded connection strings or keys
- **Environment flexibility**: Works with local emulator and production Azure
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

#### Production Configuration

In production, the application uses `DefaultAzureCredential` which supports:

- **Managed Identity** (when running in Azure)
- **Service Principal** (via environment variables)
- **Azure CLI** (for local development)
- **Visual Studio credentials**

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
- Azure Service Bus Emulator: Verifies the service is running on port 5300

### Connection Details

The application is configured to connect to these local services via the `appsettings.json` file:

- **SQL Server**: `Server=localhost,1433;Database=WolverineDemo;User Id=sa;Password=Password123!;TrustServerCertificate=true`
- **Azure Service Bus**: Uses FQDN `localhost:44444` with Azure Identity token authentication

### Service Bus Emulator Configuration

The Service Bus emulator is configured using `config.json` which includes:

- A default namespace: `wolverine-demo-asb`

You can modify `config.json` to add more queues, topics, and subscriptions as needed for your development.

### Troubleshooting

If you encounter connection issues:

1. Ensure Docker is running
2. Check if containers are healthy: `docker-compose ps`
3. View logs: `docker-compose logs [service-name]`
4. Restart services: `docker-compose restart`
5. Verify Azure CLI login: `az account show`
6. Check Service Bus emulator health: `curl http://localhost:5300`

### Database Initialization

The SQL Server container will start with an empty database. You may need to run your application's database initialization/migration scripts to create the required tables and schema.

### Azure Service Bus Emulator Notes

This setup uses the official Microsoft Azure Service Bus emulator container (`mcr.microsoft.com/azure-messaging/servicebus-emulator:latest`) as documented in the [Microsoft documentation](https://learn.microsoft.com/en-us/azure/service-bus-messaging/test-locally-with-service-bus-emulator?source=recommendations&tabs=docker-linux-container). The emulator provides a local development environment that closely mimics the Azure Service Bus service, allowing you to develop and test your messaging functionality locally.

The emulator runs on port 44444 (mapped from the container's internal port 5672) and includes a management interface on port 5300 for monitoring and debugging.

### Azure Identity Benefits

Using Azure Identity instead of connection strings provides several advantages:

- **Security**: No hardcoded secrets in configuration files
- **Compliance**: Follows Azure security best practices
- **Flexibility**: Easy to switch between development and production environments
- **Auditing**: Better tracking of service access and authentication
- **Rotation**: Automatic credential rotation when using Managed Identity
