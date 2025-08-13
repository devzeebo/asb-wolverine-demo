# JasperFx Options

JasperFx.JasperFxOptions

- ApplicationAssembly: WolverineDemo.Api 1.0.0.0
- DefaultCommand: run
- DevelopmentEnvironmentName: Development
- Factory: None
- GeneratedCodeOutputPath: /home/devzeebo/git/wolverine-demo/WolverineDemo.Api/Internal/Generated
- OptionsFile: None
- ServiceName: WolverineDemo.Api
- SubjectUri: system://jasperfx/
- TenantIdStyle: CaseSensitive
- Title: JasperFx Options
- ActiveProfile
  - AssertAllPreGeneratedTypesExist: False
  - GeneratedCodeMode: Dynamic
  - GeneratedCodeOutputPath: /home/devzeebo/git/wolverine-demo/WolverineDemo.Api/bin/Debug/net8.0/Internal/Generated
  - ResourceAutoCreate: CreateOrUpdate
  - SourceCodeWritingEnabled: True
- Development
  - AssertAllPreGeneratedTypesExist: False
  - GeneratedCodeMode: Dynamic
  - GeneratedCodeOutputPath: /home/devzeebo/git/wolverine-demo/WolverineDemo.Api/bin/Debug/net8.0/Internal/Generated
  - ResourceAutoCreate: CreateOrUpdate
  - SourceCodeWritingEnabled: True
- Production
  - AssertAllPreGeneratedTypesExist: False
  - GeneratedCodeMode: Dynamic
  - GeneratedCodeOutputPath: /home/devzeebo/git/wolverine-demo/WolverineDemo.Api/bin/Debug/net8.0/Internal/Generated
  - ResourceAutoCreate: CreateOrUpdate
  - SourceCodeWritingEnabled: True

# Wolverine

info: Wolverine.Runtime.WolverineRuntime[0]
Starting Wolverine messaging for application assembly WolverineDemo.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
info: Wolverine.Runtime.WolverineRuntime[0]
The Wolverine code generation mode is Dynamic. This is suitable for development, but you may want to opt into other options for production usage to reduce start up time and resource utilization.
info: Wolverine.Runtime.WolverineRuntime[0]
See https://wolverine.netlify.app/guide/codegen.html for more information
info: Wolverine.Configuration.HandlerDiscovery[0]
Searching assembly WolverineDemo.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null for Wolverine message handlers
info: Wolverine.Runtime.WolverineRuntime[0]
Wolverine assigned node id for envelope persistence is 1420269283

# Wolverine Options

Service Name: WolverineDemo.Api

- DefaultExecutionTimeout
  - 00:01:00
- AutoBuildMessageStorageOnStartup
  - None
- TypeLoadMode
  - Dynamic
- ExternalTransportsAreStubbed
  - True
- Assemblies
  - WolverineDemo.Api (application)
- Extensions
  - Wolverine.SqlServer.SqlServerBackedPersistence
  - Wolverine.EntityFrameworkCore.Internals.EntityFrameworkCoreBackedPersistence
- Serializers

| Content Type     | Serializer                         |
| ---------------- | ---------------------------------- |
| binary/envelope  | EnvelopeReaderWriter               |
| binary/wolverine | IntrinsicSerializer                |
| application/json | SystemTextJsonSerializer (default) |

# Handler Discovery Rules

- **Assemblies**
  - WolverineDemo.Api
- **Handler Type Rules**
  - **Include:**
    - Name ends with 'Handler'
    - Name ends with 'Consumer'
    - Inherits from Wolverine.Saga
    - Implements Wolverine.IWolverineHandler
    - Has attribute Wolverine.Attributes.WolverineHandlerAttribute
  - **Exclude:**
    - Not GeneratedStreamStateQueryHandler
    - Is not a public type
    - Has attribute Wolverine.Attributes.WolverineIgnoreAttribute
- **Handler Method Rules**
  - **Include:**
    - Method name is 'Handle' (case sensitive)
    - Method name is 'HandleAsync' (case sensitive)
    - Method name is 'Handles' (case sensitive)
    - Method name is 'HandlesAsync' (case sensitive)
    - Method name is 'Consume' (case sensitive)
    - Method name is 'ConsumeAsync' (case sensitive)
    - Method name is 'Consumes' (case sensitive)
    - Method name is 'ConsumesAsync' (case sensitive)
    - Method name is 'Orchestrate' (case sensitive)
    - Method name is 'OrchestrateAsync' (case sensitive)
    - Method name is 'Orchestrates' (case sensitive)
    - Method name is 'OrchestratesAsync' (case sensitive)
    - Method name is 'Start' (case sensitive)
    - Method name is 'StartAsync' (case sensitive)
    - Method name is 'Starts' (case sensitive)
    - Method name is 'StartsAsync' (case sensitive)
    - Method name is 'StartOrHandle' (case sensitive)
    - Method name is 'StartOrHandleAsync' (case sensitive)
    - Method name is 'StartsOrHandles' (case sensitive)
    - Method name is 'StartsOrHandlesAsync' (case sensitive)
    - Method name is 'NotFound' (case sensitive)
    - Method name is 'NotFoundAsync' (case sensitive)
    - Has attribute [WolverineHandler]
  - **Exclude:**
    - Method is declared by object
    - IDisposable.Dispose()
    - IAsyncDisposable.DisposeAsync()
    - Contains Generic Parameters
    - Special Name
    - Has attribute [WolverineIgnore]
    - Has no arguments
    - Cannot determine a valid message type
    - Returns a primitive type

# Message Handlers

| Message Name                                      | Message Type                            | Handler.Method()                                                                  | Generated Type Name     |
| ------------------------------------------------- | --------------------------------------- | --------------------------------------------------------------------------------- | ----------------------- |
| TestHandler_WolverineDemo.Api.TestHandler+Command | TestHandler.Command (WolverineDemo.Api) | TestHandler.Handle(command, cancellationToken.EscapeMarkup()) (WolverineDemo.Api) | CommandHandler279873427 |

# Message Routing

| Message Type                           | Destination                        | Content Type     |
| -------------------------------------- | ---------------------------------- | ---------------- |
| Wolverine.Runtime.Agents.IAgentCommand | local://agents/                    | application/json |
| WolverineDemo.Api.TestHandler.Command  | asb://queue/wolverinedemo.api.test | application/json |

# Subscriptions

| Uri                                     | Name                        | Mode             | Serializer(s)                                 |
| --------------------------------------- | --------------------------- | ---------------- | --------------------------------------------- |
| asb://queue/wolverine-dead-letter-queue | wolverine-dead-letter-queue | BufferedInMemory | IntrinsicSerializer, SystemTextJsonSerializer |
| asb://queue/wolverine.control.144976977 | Control                     | BufferedInMemory | IntrinsicSerializer, SystemTextJsonSerializer |
| asb://queue/wolverinedemo.api.test      | WolverineDemo.Api.Test      | BufferedInMemory | IntrinsicSerializer, SystemTextJsonSerializer |
| local://agents/                         | agents                      | BufferedInMemory | IntrinsicSerializer, SystemTextJsonSerializer |
| local://default/                        | default                     | BufferedInMemory | IntrinsicSerializer, SystemTextJsonSerializer |
| local://durable/                        | durable                     | Durable          | IntrinsicSerializer, SystemTextJsonSerializer |
| local://replies/                        | replies                     | BufferedInMemory | IntrinsicSerializer, SystemTextJsonSerializer |
| local://scheduled/                      | scheduled                   | Durable          | IntrinsicSerializer, SystemTextJsonSerializer |

# Listeners

| Uri                                      | Name                   | Mode             | Execution                                        | Serializers                                   |
| ---------------------------------------- | ---------------------- | ---------------- | ------------------------------------------------ | --------------------------------------------- |
| local://agents/                          | agents                 | BufferedInMemory | MaxDegreeOfParallelism: 20, EnsureOrdered: False | IntrinsicSerializer, SystemTextJsonSerializer |
| asb://queue/wolverine.control.1843933656 | Control                | BufferedInMemory | MaxDegreeOfParallelism: 16, EnsureOrdered: False | IntrinsicSerializer, SystemTextJsonSerializer |
| local://default/                         | default                | BufferedInMemory | MaxDegreeOfParallelism: 16, EnsureOrdered: False | IntrinsicSerializer, SystemTextJsonSerializer |
| local://durable/                         | durable                | Durable          | MaxDegreeOfParallelism: 16, EnsureOrdered: False | IntrinsicSerializer, SystemTextJsonSerializer |
| local://replies/                         | replies                | BufferedInMemory | MaxDegreeOfParallelism: 16, EnsureOrdered: False | IntrinsicSerializer, SystemTextJsonSerializer |
| local://scheduled/                       | scheduled              | Durable          | MaxDegreeOfParallelism: 16, EnsureOrdered: False | IntrinsicSerializer, SystemTextJsonSerializer |
| asb://queue/wolverinedemo.api.test       | WolverineDemo.Api.Test | Durable          | MaxDegreeOfParallelism: 16, EnsureOrdered: False | IntrinsicSerializer, SystemTextJsonSerializer |

Error HandlingFailure rules specific to a message type
are applied before the global failure rules

# Global Failure Rules

- All exceptions
  - Move to Error Queue
