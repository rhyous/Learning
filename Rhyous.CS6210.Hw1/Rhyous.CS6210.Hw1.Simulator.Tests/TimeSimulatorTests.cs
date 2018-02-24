using System;
using Rhyous.CS6210.Hw1.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.CS6210.Hw1.Simulator.Tests
{
    [TestClass]
    public class TimeSimulatorTests
    {
        [TestMethod]
        public void TimeSimulatorStartTest()
        {
            // Arrange
            var timeSimulator = new TimeSimulator();
            int callCount = 0;

            // Act
            timeSimulator.Start(DateTime.Now, 43200, 1, (DateTime d) => 
                        {
                            callCount++;
                            timeSimulator.IsReportingProgress = false;
                        } );
            timeSimulator.Wait();

            // Assert
            Assert.IsTrue(callCount > 0 && callCount < 4, "There should be between 1 and 3 calls");
        }
    }
}
