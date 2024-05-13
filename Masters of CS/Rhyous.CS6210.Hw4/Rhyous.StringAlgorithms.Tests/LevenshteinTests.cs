using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.StringAlgorithms.Tests
{
    [TestClass]
    public class LevenshteinTests
    {
        [TestMethod]
        public void WagnerFischer_Sitting_Kitten_Test()
        {
            // Arrange
            var miString = "sitting";
            var njString = "kitten";
            int[,] expected = new int[,]
            {  //         k  i  t  t  e  n
               /* */ { 0, 1, 2, 3, 4, 5, 6},
               /*s*/ { 1, 1, 2, 3, 4, 5, 6},
               /*i*/ { 2, 2, 1, 2, 3, 4, 5},
               /*t*/ { 3, 3, 2, 1, 2, 3, 4},
               /*t*/ { 4, 4, 3, 2, 1, 2, 3},
               /*i*/ { 5, 5, 4, 3, 2, 2, 3},
               /*n*/ { 6, 6, 5, 4, 3, 3, 2},
               /*g*/ { 7, 7, 6, 5, 4, 4, 3}
            };

            // Act
            var actual = Levenshtein.WagnerFischer(miString, njString);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WagnerFischer_Sunday_Saturday_Test()
        {
            // Arrange
            var miString = "sunday";
            var njString = "saturday";
            int[,] expected = new int[,]
            {  //         s  a  t  u  r  d  a  y
               /* */ {0, 1, 2, 3, 4, 5, 6, 7, 8},
               /*s*/ {1, 0, 1, 2, 3, 4, 5, 6, 7},
               /*u*/ {2, 1, 1, 2, 2, 3, 4, 5, 6},
               /*n*/ {3, 2, 2, 2, 3, 3, 4, 5, 6},
               /*d*/ {4, 3, 3, 3, 3, 4, 3, 4, 5},
               /*a*/ {5, 4, 3, 4, 4, 4, 4, 3, 4},
               /*y*/ {6, 5, 4, 4, 5, 5, 5, 4, 3},
            };
            
            // Act
            var actual = Levenshtein.WagnerFischer(miString, njString);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
