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

            return app;
        }
    }
}
