using AwesomeAssertions;
using WolverineDemo.Api;

namespace WolverineDemo.Tests;

public class AzureServiceBusQueueNamesTests
{
    [Fact]
    public void Nested_command_is_HANDLER_name()
    {
        new TestContext()
            .Given_command_is_nested_in_handler()
            .When_getting_asb_name()
            .Then_name_is_namespace_dot_HANDLER_name();
    }

    [Fact]
    public void Non_nested_command_is_TYPE_name()
    {
        new TestContext()
            .Given_command_is_not_nested_in_handler()
            .When_getting_asb_name()
            .Then_name_is_namespace_dot_TYPE_name();
    }

    [Fact]
    public void Command_with_null_namespace_uses_default()
    {
        new TestContext()
            .Given_command_has_NULL_namespace()
            .When_getting_asb_name()
            .Then_name_is_DEFAULT_dot_TYPE_name();
    }

    [Fact]
    public void Command_in_non_handler_declaring_type_uses_TYPE_name()
    {
        new TestContext()
            .Given_command_is_in_non_handler_declaring_type()
            .When_getting_asb_name()
            .Then_name_is_namespace_dot_TYPE_name();
    }

    [Fact]
    public void Command_with_service_bus_attribute_uses_attribute_queue_name()
    {
        new TestContext()
            .Given_command_has_service_bus_attribute_with_queue_name()
            .When_getting_asb_name()
            .Then_name_is_ATTRIBUTE_queue_name();
    }

    [Fact]
    public void Command_without_service_bus_attribute_returns_null()
    {
        new TestContext()
            .Given_command_has_no_service_bus_attribute()
            .When_getting_asb_name()
            .Then_name_is_NULL();
    }

    class TestContext
    {
        public Type command_type = null!;
        public string? queue_name = null!;

        public TestContext Given_command_is_nested_in_handler()
        {
            command_type = typeof(MockHandler.Command);
            return this;
        }

        public TestContext Given_command_is_not_nested_in_handler()
        {
            command_type = typeof(CommandName);
            return this;
        }

        public TestContext Given_command_has_NULL_namespace()
        {
            command_type = typeof(NullNamespaceCommand);
            return this;
        }

        public TestContext Given_command_is_in_non_handler_declaring_type()
        {
            command_type = typeof(NonHandlerType.CommandName);
            return this;
        }

        public TestContext Given_command_has_service_bus_attribute_with_queue_name()
        {
            command_type = typeof(AttributedCommand);
            return this;
        }

        public TestContext Given_command_has_no_service_bus_attribute()
        {
            command_type = typeof(PlainCommand);
            return this;
        }

        public TestContext When_getting_asb_name()
        {
            queue_name = AzureServiceBusQueueNames.GetAzureServiceBusQueueName(command_type);
            return this;
        }

        public TestContext Then_name_is_namespace_dot_HANDLER_name()
        {
            queue_name.Should().Be("WolverineDemo.Tests.Mock");
            return this;
        }

        public TestContext Then_name_is_namespace_dot_TYPE_name()
        {
            queue_name.Should().Be("WolverineDemo.Tests.CommandName");
            return this;
        }

        public TestContext Then_name_is_DEFAULT_dot_TYPE_name()
        {
            queue_name.Should().Be("Default.NullNamespaceCommand");
            return this;
        }

        public TestContext Then_name_is_ATTRIBUTE_queue_name()
        {
            queue_name.Should().Be("CustomQueueName");
            return this;
        }

        public TestContext Then_name_is_NULL()
        {
            queue_name.Should().BeNull();
            return this;
        }
    }

    class MockHandler
    {
        [ServiceBusMessage]
        public record Command;
    }

    [ServiceBusMessage]
    record CommandName;

    class NonHandlerType
    {
        [ServiceBusMessage]
        public record CommandName;
    }

    [ServiceBusMessage(QueueName = "CustomQueueName")]
    record AttributedCommand;

    record PlainCommand;
}
