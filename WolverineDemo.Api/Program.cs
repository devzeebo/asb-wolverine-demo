using System.Reflection;
using Azure.Identity;
using JasperFx;
using Microsoft.EntityFrameworkCore;
using Wolverine;
using Wolverine.AzureServiceBus;
using Wolverine.EntityFrameworkCore;
using Wolverine.SqlServer;
using WolverineDemo.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql")!));

builder.Services.AddWolverine(opts =>
{
    opts.Discovery.IncludeAssembly(typeof(TestHandler).Assembly);

    // var asbConnection = builder.Configuration.GetValue<string>("Wolverine:ServiceBus:ConnectionString")!;
    var asbFqdn = builder.Configuration.GetValue<string>("Wolverine:ServiceBus:FQDN")!;
    opts.UseAzureServiceBus(asbFqdn, new DefaultAzureCredential())
    // opts.UseAzureServiceBus(asbConnection)
        .AutoProvision()
        .EnableWolverineControlQueues()
        .UseConventionalRouting(convention => convention
            .IncludeTypes(type => type.GetTypeInfo().GetCustomAttribute<ServiceBusMessageAttribute>() is not null)
            .QueueNameForListener(GetAzureServiceBusQueueName)
            .QueueNameForSender(GetAzureServiceBusQueueName));

    opts.Policies.UseDurableOutboxOnAllSendingEndpoints();
    opts.Policies.UseDurableInboxOnAllListeners();
    opts.Policies.UseDurableLocalQueues();

    opts.Policies
        .ConfigureConventionalLocalRouting()
        .Named(QueueName);

    opts.PersistMessagesWithSqlServer(builder.Configuration.GetConnectionString("sql")!, "Wolverine")
        .EnableCommandQueues(false);

    opts.ListenToAzureServiceBusQueue(GetAzureServiceBusQueueName(typeof(TestHandler.Command))!)
        .RequireSessions()
        .Sequential();
    opts.PublishMessage<TestHandler.Command>()
        .ToAzureServiceBusQueue(GetAzureServiceBusQueueName(typeof(TestHandler.Command))!)
        .RequireSessions();

    opts.Policies.AutoApplyTransactions();
    opts.UseEntityFrameworkCoreTransactions();

    opts.AutoBuildMessageStorageOnStartup = AutoCreate.None;

    Console.WriteLine(opts.DescribeHandlerMatch(typeof(TestHandler)));

    string? GetAzureServiceBusQueueName(Type messageType)
    {
        if (messageType.GetTypeInfo().GetCustomAttribute<ServiceBusMessageAttribute>() is var attr && attr is null)
        {
            return null;
        }
        return attr?.QueueName ?? QueueName(messageType);
    }

    string QueueName(Type messageType)
    {
        var ns = messageType.Namespace ?? "Default";

        if (messageType.DeclaringType?.Name.EndsWith("Handler", StringComparison.OrdinalIgnoreCase) == true)
        {
            var baseName = messageType.DeclaringType.Name[0..^"Handler".Length];
            return $"{ns}.{baseName}";
        }

        return $"{ns}.{messageType.Name}";
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/test", TestEndpoint.Test);

await app.RunJasperFxCommands(args);
