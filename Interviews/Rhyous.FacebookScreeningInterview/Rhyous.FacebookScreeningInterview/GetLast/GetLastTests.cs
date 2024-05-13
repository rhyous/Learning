using Newtonsoft.Json.Linq;

namespace Rhyous.FacebookScreeningInterview.GetLast
{
    [TestClass]
    public class GetLastTests
    {
        [TestMethod]
        public void GetLastWorker_Get_Empty_ReturnsDefault_NullString()
        {
            // Arrange
            var worker = new GetLastWorker<int, string>();

            // Act
            var actual = worker.Get(1);

            // Assert
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void GetLastWorker_Last_Empty_ReturnsDefault_NullString()
        {
            // Arrange
            var worker = new GetLastWorker<int, string>();

            // Act
            var actual = worker.Get(1);

            // Assert
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void GetLastWorker_MultiStepProcess_Works()
        {
            // Arrange
            var worker = new GetLastWorker<int, string>();

            // Act && Assert
            worker.Get(1);
            worker.Set(1, "1");
            var last1 = worker.Last();
            Assert.AreEqual("1", last1, "Last should be 1");
            worker.Set(2, "2");
            var last2 = worker.Last();
            Assert.AreEqual("2", last2, "Last should be 2");
            worker.Set(3, "3");
            var last3 = worker.Last();
            Assert.AreEqual("3", last3, "Last should be 3");
            worker.Remove(3);
            var last4 = worker.Last();
            Assert.AreEqual("2", last4, "Last should be 2");
        }
    }
}
