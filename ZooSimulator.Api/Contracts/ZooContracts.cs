namespace ZooSimulator.Api.Contracts
{
    public record ZooStatus(DateTimeOffset Time, IReadOnlyList<AnimalStatus> Animals);

    public record AnimalStatus(string Name, string Type, double Health, bool IsAlive, bool CanWalk);

    public record FeedResult(double MonkeyPercent, double GiraffePercent, double ElephantPercent);
}
