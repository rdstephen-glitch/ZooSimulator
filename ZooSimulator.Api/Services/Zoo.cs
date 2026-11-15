using ZooSimulator.Api.Contracts;
using ZooSimulator.Api.Models;

namespace ZooSimulator.Api.Services
{
    public class Zoo
    {
        private static readonly string[] MonkeyNames =
            ["Bongo", "Coco", "Mango", "Banjo", "Kiki", "Zippy", "Biscuit", "Pip", "Chico", "Milo", "Ziggy", "Toffee", "James", "Oreo"];

        private static readonly string[] GiraffeNames =
            ["Gerry", "Daisy", "Stretch", "Tallulah", "Jazz", "Willow", "Twiga", "Clover", "Marlow", "Sunny", "Dotty", "Lofty", "Savannah", "Rob"];

        private static readonly string[] ElephantNames =
            ["Ellie", "Tembo", "Peanut", "Bruno", "Maisie", "Otis", "Rosie", "Winston", "Nellie", "Boris", "Poppy", "Humphrey", "Alecia"];

        private const int AnimalsPerType = 5;

        private const double MinReduceHealthPercent = 0;
        private const double MaxReduceHealthPercent = 20;

        private const double MinFeedPercent = 10;
        private const double MaxFeedPercent = 25;

        private readonly object _lock = new();
        private readonly IRandomValue _random;
        private readonly List<Animal> _animals = [];
        private readonly TimeProvider _timeProvider;

        public IReadOnlyList<Animal> Animals => _animals;
        public DateTimeOffset CurrentTime { get; private set; }

        public Zoo(IRandomValue random, TimeProvider timeProvider)
        {
            _random = random;
            _timeProvider = timeProvider;
            Populate();
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

        public TypeFeedResult? FeedType(string type)
        {
            lock (_lock)
            {
                var matching = _animals
                    .Where(a => a.GetType().Name.Equals(type, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (matching.Count == 0) return null;

                var roll = _random.ReturnRandomValue(MinFeedPercent, MaxFeedPercent);

                foreach (var animal in matching)
                {
                    animal.Feed(roll);
                }

                return new TypeFeedResult(matching[0].GetType().Name, roll);
            }
        }

        public void Reset()
        {
            lock (_lock)
            {
                Populate();
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
        private void Populate()
        {
            _animals.Clear();
            CurrentTime = _timeProvider.GetUtcNow();

            var monkeyNames = PickNames(MonkeyNames, AnimalsPerType);
            var giraffeNames = PickNames(GiraffeNames, AnimalsPerType);
            var elephantNames = PickNames(ElephantNames, AnimalsPerType);

            for (var i = 0; i < AnimalsPerType; i++)
            {
                _animals.Add(new Monkey(monkeyNames[i]));
                _animals.Add(new Giraffe(giraffeNames[i]));
                _animals.Add(new Elephant(elephantNames[i]));
            }
        }

        private static string[] PickNames(string[] pool, int count)
        {
            var shuffled = pool.ToArray();
            Random.Shared.Shuffle(shuffled);
            return shuffled.Take(count).ToArray();
        }
    }
}
