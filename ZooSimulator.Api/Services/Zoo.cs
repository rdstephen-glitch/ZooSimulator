using ZooSimulator.Api.Models;

namespace ZooSimulator.Api.Services
{
    public class Zoo
    {
        private const int AnimalsPerType = 5;
        private const double MinReduceHealthPercent = 0;
        private const double MaxReduceHealthPercent = 20;

        private readonly IRandomValue _random;
        private readonly List<Animal> _animals = [];

        public Zoo(IRandomValue random)
        {
            _random = random;

            for (var i = 1; i <= AnimalsPerType; i++)
            {
                _animals.Add(new Monkey($"Monkey {i}"));
                _animals.Add(new Giraffe($"Giraffe {i}"));
                _animals.Add(new Elephant($"Elephant {i}"));
            }
        }

        public IReadOnlyList<Animal> Animals => _animals;

        public void HourlyTick()
        {
            foreach (var animal in _animals)
            {
                animal.ReduceHealth(_random.ReturnRandomValue(MinReduceHealthPercent, MaxReduceHealthPercent));
            }
        }
    }
}
