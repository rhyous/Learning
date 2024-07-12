using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.FacebookScreeningInterview.Valley
{
    [TestClass]
    public class ValleyUnitTests
    {
        [TestMethod]
        public void Valley_GetValley_Test1()
        {
            // Arrange
            var array = new int[] { 1 };
            var valley = new Valley();

            // Act
            var result = valley.GetValley(array);

            // Assert
            Assert.AreEqual(0, result.Index);
            Assert.AreEqual(1, result.Value);
        }

        [TestMethod]
        public void Valley_GetValley_Test2()
        {
            // Arrange
            var array = new int[] { 2, 1 };
            var valley = new Valley();

            // Act
            var result = valley.GetValley(array);

            // Assert
            Assert.AreEqual(1, result.Index);
            Assert.AreEqual(1, result.Value);
        }

        [TestMethod]
        public void Valley_GetValley_Test3()
        {
            // Arrange
            var array = new int[] { 2, 1, 2};
            var valley = new Valley();

            // Act
            var result = valley.GetValley(array);

            // Assert
            Assert.AreEqual(1, result.Index);
            Assert.AreEqual(1, result.Value);
        }

        [TestMethod]
        public void Valley_GetValley_Test_AscendingNumbers()
        {
            // Arrange
            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var valley = new Valley();

            // Act
            var result = valley.GetValley(array);

            // Assert
            Assert.AreEqual(0, result.Index);
            Assert.AreEqual(1, result.Value);
        }

        [TestMethod]
        public void Valley_GetValley_Test_DescendingNumbers()
        {
            // Arrange
            var array = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            var valley = new Valley();

            // Act
            var result = valley.GetValley(array);

            // Assert
            Assert.AreEqual(9, result.Index);
            Assert.AreEqual(1, result.Value);
        }

        [TestMethod]
        public void Valley_GetValley_Test_ScrambledNumbers_GoLeft()
        {
            // Arrange
            var array = new int[] { 4, 3, 10, 9, 6, 7, 8, 5, 2, 1 };
            var valley = new Valley();

            // Act
            var result = valley.GetValley(array);

            // Assert
            Assert.AreEqual(0, result.Index);
            Assert.AreEqual(4, result.Value);
        }

        [TestMethod]
        public void Valley_GetValley_Test_ScrambledNumbers_GoRight()
        {
            // Arrange
            var array = new int[] { 4, 3, 10, 6, 9, 8, 7, 5, 2, 1 };
            var valley = new Valley();

            // Act
            var result = valley.GetValley(array);

            // Assert
            Assert.AreEqual(9, result.Index);
            Assert.AreEqual(1, result.Value);
        }


        #region Get Sub Arrays
        [TestMethod]
        public void Valley_GetLeftSubArray_Test()
        {
            // Arrange
            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var valley = new Valley();

            // Act
            var result = valley.GetLeftSubArray(array, array.Length / 2);

            // Assert
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4 }, result);

        }

        [TestMethod]
        public void Valley_GetRightSubArray_Test()
        {
            // Arrange
            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var valley = new Valley();

            // Act
            var result = valley.GetRightSubArray(array, array.Length / 2);

            // Assert
            CollectionAssert.AreEqual(new[] { 7, 8, 9, 10 }, result);

        }
        #endregion

    }
}
