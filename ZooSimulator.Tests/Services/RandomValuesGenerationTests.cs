using System;
using System.Collections.Generic;
using System.Text;
using ZooSimulator.Api.Services;

namespace ZooSimulator.Tests.Services
{
    public class RandomValuesGenerationTests(params double[] values) : IRandomValue
    {
        private readonly Queue<double> _values = new(values);

        public List<(double Min, double Max)> Requests { get; } = [];

        public double ReturnRandomValue(double min, double max)
        {
            Requests.Add((min, max));
            return _values.Dequeue();
        }
    }
}
