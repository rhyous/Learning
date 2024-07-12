using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.FacebookScreeningInterview.Parenthesis
{
    [TestClass]
    public class BalanceParenthesisTests
    {
        [TestMethod]
        [PrimitiveList("()", "(()", "())", "(((()", "()))))")]
        public void BalanceParenthesis_Balance_Test1(string str)
        {
            // Arrange
            var balanceParenthesis = new BalanceParenthesis();


            // Act
            var result = balanceParenthesis.Balance(str);

            // Assert
            Assert.AreEqual("()", result);
        }

        [TestMethod]
        [PrimitiveList("(())", "(((())", "(()))))")]
        public void BalanceParenthesis_Balance_Test2(string str)
        {
            // Arrange
            var balanceParenthesis = new BalanceParenthesis();

            // Act
            var result = balanceParenthesis.Balance(str);

            // Assert
            Assert.AreEqual("(())", result);
        }

        [TestMethod]
        [PrimitiveList("((abc))", "((((abc))", "((abc)))))")]
        public void BalanceParenthesis_Balance_Test3(string str)
        {
            // Arrange
            var balanceParenthesis = new BalanceParenthesis();

            // Act
            var result = balanceParenthesis.Balance(str);

            // Assert
            Assert.AreEqual("((abc))", result);
        }
    }
}
