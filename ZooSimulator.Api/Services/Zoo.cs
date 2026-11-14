using ZooSimulator.Api.Contracts;
using ZooSimulator.Api.Models;

namespace ZooSimulator.Api.Services
{
    public class Zoo
    {
        private const int AnimalsPerType = 5;

        private const double MinReduceHealthPercent = 0;
        private const double MaxReduceHealthPercent = 20;

        private const double MinFeedPercent = 10;
        private const double MaxFeedPercent = 25;

        private readonly object _lock = new();
        private readonly IRandomValue _random;
        private readonly List<Animal> _animals = [];

        public IReadOnlyList<Animal> Animals => _animals;
        public DateTimeOffset CurrentTime { get; private set; }

        public Zoo(IRandomValue random, TimeProvider timeProvider)
        {
            _random = random;
            CurrentTime = timeProvider.GetUtcNow();

            for (var i = 1; i <= AnimalsPerType; i++)
            {
                _animals.Add(new Monkey($"Monkey {i}"));
                _animals.Add(new Giraffe($"Giraffe {i}"));
                _animals.Add(new Elephant($"Elephant {i}"));
            }
        }

        public void SimulateTime()
        {
            lock (_lock)
            foreach (var animal in _animals)
            {
                animal.ReduceHealth(_random.ReturnRandomValue(MinReduceHealthPercent, MaxReduceHealthPercent));
            }

            CurrentTime = CurrentTime.AddHours(1);
        }

        public FeedResult FeedAnimals()
        {
            lock (_lock)
            {
                var monkeyRoll = _random.ReturnRandomValue(MinFeedPercent, MaxFeedPercent);
                var giraffeRoll = _random.ReturnRandomValue(MinFeedPercent, MaxFeedPercent);
                var elephantRoll = _random.ReturnRandomValue(MinFeedPercent, MaxFeedPercent);

                foreach (var animal in _animals)
                {
                    var roll = animal switch
                    {
                        Monkey => monkeyRoll,
                        Giraffe => giraffeRoll,
                        Elephant => elephantRoll,
                        _ => throw new NotImplementedException()
                    };

                    animal.Feed(roll);
                }

                return new FeedResult(monkeyRoll, giraffeRoll, elephantRoll);
            }
        }

        public ZooStatus GetStatus()
        {
            lock (_lock)
            {
                var animals = _animals
                    .Select(a => new AnimalStatus(
                        a.Name,
                        a.GetType().Name,
                        a.Health,
                        a.IsAlive,
                        a.IsAlive && (a is not Elephant e || e.CanWalk)))
                    .ToList();

                return new ZooStatus(CurrentTime, animals);
            }
        }
    }
}
