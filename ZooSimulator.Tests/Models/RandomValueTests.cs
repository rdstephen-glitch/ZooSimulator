using System;
using System.Collections.Generic;
using System.Text;
using ZooSimulator.Api.Services;

namespace ZooSimulator.Tests.Models
{
    public class RandomValueTests
    {
        [Theory]
        [InlineData(0, 20)]
        [InlineData(10, 25)]
        public void ReturnRandomValueInRange(double min, double max)
        {
            var random = new RandomSource();

            for (var i = 0; i < 1000; i++)
            {
                var value = random.ReturnRandomValue(min, max);

                Assert.InRange(value, min, max);
            }
        }
    }
}
