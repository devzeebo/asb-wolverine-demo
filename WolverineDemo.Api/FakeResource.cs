using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace WolverineDemo.Api;

public class FakeResourceService
{
    readonly Dictionary<string, FakeResource> _resources = [];

    public bool TryGetResource(string id, [NotNullWhen(true)] out FakeResource resource)
    {
        return _resources.TryGetValue(id, out resource!);
    }

    public Task<FakeResource> FetchResource(string id, CancellationToken cancellationToken)
    {
        _resources[id] = new()
        {
            Id = id,
            TransactionId = Guid.NewGuid().ToString()
        };

        return Task.FromResult(_resources[id]);
    }
}

public class FakeResource
{
    public required string Id { get; init; }
    public required string TransactionId { get; init; }
}