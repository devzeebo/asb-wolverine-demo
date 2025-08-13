using System.Reflection;
using System.Threading.Tasks;

namespace WolverineDemo.Api;

public class TestHandler
{
    [ServiceBusMessage]
    public record Command
    {
        public required string Id { get; set; }
    }

    public record Response
    {
        public required string? TransactionId { get; set; }
    }

    public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
    {
        if (!_service.TryGetResource(command.Id, out var resource))
        {
            resource = await _service.FetchResource(command.Id, cancellationToken);
        }

        return new() { TransactionId = resource.TransactionId };
    }

    readonly FakeResourceService _service;
    public TestHandler(FakeResourceService service)
    {
        _service = service;
    }
}