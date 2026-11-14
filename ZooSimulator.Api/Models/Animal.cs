namespace ZooSimulator.Api.Models
{
    public abstract class Animal
    {
        protected Animal(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public double Health { get; set; } = 100;

        public bool IsAlive { get; private set; } = true;

        public void ReduceHealth(double percent)
        {
            if (!IsAlive) return;

            Health -= Health * (percent / 100);
            ApplyHealthRules();
        }

        public void Feed(double percent)
        {
            if (!IsAlive) return;

            Health = Math.Min(100, Health + Health * (percent / 100));

            OnFed();
        }

        protected void Terminate() => IsAlive = false;

        protected abstract void ApplyHealthRules();

        protected virtual void OnFed() { }

    }
}
