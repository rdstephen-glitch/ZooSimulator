namespace ZooSimulator.Api.Models
{
    public class Monkey : Animal
    {
        private const double DeathThreshold = 30;
        public Monkey(string name) : base(name) { }

        protected override void ApplyHealthRules()
        {
            if (Health < DeathThreshold) Terminate();
        }
    }
}
