namespace ZooSimulator.Api.Services
{
    public class RandomValue : IRandomValue
    {
        //Cater for damage and feeding
        public double ReturnRandomValue(double min, double max)
        {
            return min + Random.Shared.NextDouble() * (max - min);
        }
    }
}
