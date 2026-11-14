namespace ZooSimulator.Api.Services
{
    public class SimulationService(Zoo zoo, ILogger<SimulationService> logger) : BackgroundService
    {
        private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(20);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TickInterval);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                zoo.SimulateTime();

                /*Check the time simulation & health degradation is working*/
                logger.LogInformation(
                    "Zoo time is now {Time}. Alive: {Alive}/{Total}. First animal health: {Health:F2}%",
                    zoo.CurrentTime,
                    zoo.Animals.Count(a => a.IsAlive),
                    zoo.Animals.Count,
                    zoo.Animals[0].Health);
            }
        }
    }
}
