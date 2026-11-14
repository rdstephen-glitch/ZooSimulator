namespace ZooSimulator.Api.Models
{
    public class Giraffe : Animal
    {
        private const double DeathThreshold = 50;
        public Giraffe(string name) : base(name) { }

        protected override void ApplyHealthRules()
        {
            if (Health < DeathThreshold) Terminate();
        }
    }
}
