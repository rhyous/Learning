namespace Rhyous.FacebookScreeningInterview.PowerSet
{
    [TestClass]
    public class PowerSetBuilderTests
    {
        [TestMethod]
        public void GetPOwerSet_1Thru5_Test()
        {
            // Arrange
            var builder = new PowerSetBuilder();
            var set = new List<int> { 1, 2, 3, 4, 5 };
            var expectedSetCount = Math.Pow(2, set.Count);

            // Act
            var sets = builder.GetPowerSet(set);

            // Assert
            Assert.AreEqual(expectedSetCount, sets.Count);
        }

        [TestMethod]
        public void GetPOwerSet_1Thru10_Test()
        {
            // Arrange
            var builder = new PowerSetBuilder();
            var set = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var expectedSetCount = Math.Pow(2, set.Count);

            // Act
            var sets = builder.GetPowerSet(set);

            // Assert
            Assert.AreEqual(expectedSetCount, sets.Count);
        }
    }

}