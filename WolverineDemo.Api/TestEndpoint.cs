using Wolverine;
using Microsoft.AspNetCore.Mvc;


namespace WolverineDemo.Api;

public class TestEndpoint
{
    public static class TestApi
    {
        public record Request
        {
            public required string Id { get; init; }
        }

        public record Response
        {
            public required string TransactionId { get; init; }
        }
    }

    public static async Task<TestApi.Response> Test(
        [FromBody] TestApi.Request request,
        [FromServices] IMessageBus wolverine
    )
    {
        var response = await wolverine.InvokeAsync<TestApi.Response>(new TestHandler.Command { Id = request.Id });

        return new()
        {
            TransactionId = response.TransactionId
        };
    }
}