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
    }
}
