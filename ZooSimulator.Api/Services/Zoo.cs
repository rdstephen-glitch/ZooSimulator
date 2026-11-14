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
            foreach (var animal in _animals)
            {
                animal.ReduceHealth(_random.ReturnRandomValue(MinReduceHealthPercent, MaxReduceHealthPercent));
            }

            CurrentTime = CurrentTime.AddHours(1);
        }

        public void FeedAnimals()
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
        }
    }
}
