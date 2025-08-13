using System.Reflection;

namespace WolverineDemo.Api;

public static class AzureServiceBusQueueNames
{
    public static string? GetAzureServiceBusQueueName(Type messageType)
    {
        if (messageType.GetTypeInfo().GetCustomAttribute<ServiceBusMessageAttribute>() is var attr && attr is null)
        {
            return null;
        }
        return attr?.QueueName ?? QueueName(messageType);
    }

    public static string QueueName(Type messageType)
    {
        var ns = messageType.Namespace ?? "Default";

        if (messageType.DeclaringType?.Name.EndsWith("Handler", StringComparison.OrdinalIgnoreCase) == true)
        {
            var baseName = messageType.DeclaringType.Name[0..^"Handler".Length];
            return $"{ns}.{baseName}";
        }

        return $"{ns}.{messageType.Name}";
    }
}