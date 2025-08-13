namespace WolverineDemo.Api;

[AttributeUsage(AttributeTargets.Class)]
public class ServiceBusMessageAttribute : Attribute
{
    public string? QueueName { get; init; }
}