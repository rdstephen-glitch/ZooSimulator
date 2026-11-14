namespace ZooSimulator.Api.Models
{
    public class Animal
    {
        private Animal(string name)
        {
            Name = name;
        }
        public string Name { get; }
        public double Health { get; set; } = 100;
    }
}
