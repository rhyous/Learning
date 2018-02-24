using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.CS6210.Hw1.Models;

namespace Rhyous.CS6210.Hw1.ModelTests
{
    [TestClass]
    public class RandomTests
    {
        [TestMethod]
        public void RandomNext()
        {
            // Arrange
            var random = new CryptoRandom();

            for (int i = 0; i < 10000; i++)
            {
                // Act
                long l = random.Next();

                // Assert
                Assert.AreNotEqual(0, l);
            }
        }

        [TestMethod]
        public void RandomNextGreaterThanZero()
        {
            // Arrange
            var random = new CryptoRandom();

            for (int i = 0; i < 10000; i++)
            {
                // Act
                long l = random.Next(1, long.MaxValue);

                // Assert
                Assert.IsTrue(l > 0);
            }            
        }        
    }
}