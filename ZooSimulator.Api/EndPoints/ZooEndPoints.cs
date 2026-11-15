using ZooSimulator.Api.Services;

namespace ZooSimulator.Api.EndPoints
{
    public static class ZooEndpoints
    {
        public static IEndpointRouteBuilder MapZooEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/zoo");

            group.MapGet("/", (Zoo zoo) => zoo.GetStatus());
            group.MapPost("/feed", (Zoo zoo) => zoo.FeedAnimals());
            group.MapPost("/reset", (Zoo zoo) =>
            {
                zoo.Reset();
                return Results.NoContent();
            });
            group.MapPost("/feed/{type}", (string type, Zoo zoo) =>
                zoo.FeedType(type) is { } result
                    ? Results.Ok(result)
                    : Results.NotFound());

            return app;
        }
    }
}
