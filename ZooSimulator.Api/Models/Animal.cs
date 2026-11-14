namespace ZooSimulator.Api.Models
{
    public class Animal
    {
        protected Animal(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public double Health { get; set; } = 100;

        public void ReduceHealth(double percent)
        {
            Health -= Health * (percent / 100);
        }
    }
}
