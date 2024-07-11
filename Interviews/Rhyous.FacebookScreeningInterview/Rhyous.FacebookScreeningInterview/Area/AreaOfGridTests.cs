using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.FacebookScreeningInterview.Area
{
    [TestClass]
    public class AreaOfGridTests
    {
        [TestMethod]
        public void AreaOfGrid_All1s_Test()
        {
            // Arrange
            var grid = new int[,]
            {
                { 1, 1 },
                { 1, 1 },
                { 1, 1 }
            };
            var areaOfGrid = new AreaOfGrid();

            // Act
            var result = areaOfGrid.Get(grid);

            // Assert
            Assert.AreEqual(6, result);

        }

        [TestMethod]
        public void AreaOfGrid_All0s_Test()
        {
            // Arrange
            var grid = new int[,]
            {
                { 0, 0 },
                { 0, 0 },
                { 0, 0 }
            };
            var areaOfGrid = new AreaOfGrid();

            // Act
            var result = areaOfGrid.Get(grid);

            // Assert
            Assert.AreEqual(1, result);

        }

        [TestMethod]
        public void AreaOfGrid_Example1_Test()
        {
            // Arrange
            var grid = new int[,]
            {
                { 1, 0 },
                { 0, 1 },
                { 1, 1, }
            };
            var areaOfGrid = new AreaOfGrid();

            // Act
            var result = areaOfGrid.Get(grid);

            // Assert
            Assert.AreEqual(5, result);

        }

        [TestMethod]
        public void AreaOfGrid_Example2_Test()
        {
            // Arrange
            var grid = new int[,]
            {
                { 1, 0, 0, 1, 1, },
                { 1, 0, 0, 1, 1, },
                { 1, 0, 0, 0, 1, }
            };
            var areaOfGrid = new AreaOfGrid();

            // Act
            var result = areaOfGrid.Get(grid);

            // Assert
            Assert.AreEqual(6, result);

        }

        [TestMethod]
        public void AreaOfGrid_Example3_Test()
        {
            // Arrange
            var grid = new int[,]
            {
                { 1, 0, 0, 1, 1, },
                { 1, 1, 0, 1, 1, },
                { 1, 0, 0, 0, 1, }
            };
            var areaOfGrid = new AreaOfGrid();

            // Act
            var result = areaOfGrid.Get(grid);

            // Assert
            Assert.AreEqual(10, result);

        }
    }
}
