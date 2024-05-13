using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.CS6210.Hw1.Models;

namespace Rhyous.CS6210.Hw1.Simulator.Tests
{
    [TestClass]
    public class DiseaseRecordGeneratorTests
    {
        [TestMethod]
        public void DiseaseRecordGenerator_GenerateTests()
        {
            // Arrange
            var generator = new DiseaseRecordGenerator();
            var date = new DateTime(2018, 1, 1);
            // Act
            var record = generator.Generate(date, new Random());

            // Assert
            Assert.IsTrue(record.Id > 0);
            Assert.AreEqual(date, record.DiagnosisDate);
            Assert.IsTrue(Disease.Instance.ContainsKey(record.Id));
        }
    }
}
