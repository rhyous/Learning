using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.CS6210.Hw4.Models.Tests
{
    [TestClass]
    public class MasterTests
    {
        [TestMethod]
        public void AppendToLongTest()
        {
            // Arrange
            int i1 = 1;
            int i2 = 2;
            long expected = 0x100000002;

            // Act
            long actual = Master.AppendToLong(i1, i2);
            
            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
