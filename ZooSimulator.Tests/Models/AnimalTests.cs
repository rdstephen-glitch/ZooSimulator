using System;
using System.Collections.Generic;
using System.Text;
using ZooSimulator.Api.Models;

namespace ZooSimulator.Tests.Models
{
    public class AnimalTests
    {
        [Fact]
        public void CheckEveryAnimalHasFullHealth()
        {
            List<Animal> animals =
            [
                new Monkey("Test monkey"),
                new Giraffe("Test giraffe"),
                new Elephant("Test elephant")
            ];

            Assert.All(animals, animal => Assert.Equal(100.0, animal.Health));
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(10, 90)]
        [InlineData(20, 80)]
        public void ReduceHealth(double percent, double expectedHealth)
        {
            var monkey = new Monkey("Test monkey");

            monkey.ReduceHealth(percent);

            Assert.Equal(expectedHealth, monkey.Health, precision: 5);
        }

        [Fact]
        public void ReduceCurrentHealthAsPercentage()
        {
            var monkey = new Monkey("Test monkey");

            monkey.ReduceHealth(10); // 10% -> 100 to 90
            monkey.ReduceHealth(10); // 10% ->  90 to 81

            Assert.Equal(81.0, monkey.Health, precision: 5);
        }
    }
}
