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
    }
}
