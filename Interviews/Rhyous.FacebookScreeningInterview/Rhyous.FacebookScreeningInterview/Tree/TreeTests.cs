namespace Rhyous.FacebookScreeningInterview.Tree
{
    [TestClass]
    public class TreeTests
    {
        [TestMethod]
        public void Tree_AddNode_Test()
        {
            // Arrange
            var tree = new Graph<char>();

            // Act
            tree.AddNode('a');

            // Assert
            Assert.AreEqual(1, tree.Count);
        }

        [TestMethod]
        public void Tree_CreateTree_Test()
        {
            // Arrange
            var tree = new Graph<char>();

            // Act
            tree.AddChildren('a', ['b', 'c', 'd']);
            tree.AddChildren('b', ['e', 'f']);
            tree.AddChild('d', 'x');

            // Assert
            Assert.AreEqual(7, tree.Count);
        }

        [TestMethod]
        public void Tree_GetDeepestChildren_Test()
        {
            // Arrange
            var tree = new Graph<char>();
            tree.AddChildren('a', ['b', 'c', 'd']);
            tree.AddChildren('b', ['e', 'f']);
            tree.AddChild('d', 'x');

            // Act
            var actual = tree.GetDeepest('a');

            // Assert
            var expected = new List<char> { 'e', 'f', 'x' };
            CollectionAssert.AreEqual(expected, actual.Select(i=>i.Value).ToList());
        }

        [TestMethod]
        public void Tree_GetCommonAncestorOfDeepest_BothChildrenOfSameNode_Test()
        {
            // Arrange
            var tree = new Graph<char>();
            tree.AddChildren('a', ['b', 'c', 'd']);
            tree.AddChildren('b', ['e', 'f']);
            var deepest = tree.GetDeepest('a');

            // Act
            var commonAncestorNode = tree.GetCommonAncestor(deepest);

            // Assert
            Assert.AreEqual('b', commonAncestorNode.Value);
        }

        [TestMethod]
        public void Tree_GetCommonAncestorOfDeepest_MustGoBackToRoot_Test()
        {
            // Arrange
            var tree = new Graph<char>();
            tree.AddChildren('a', ['b', 'c', 'd']);
            tree.AddChildren('b', ['e', 'f']);
            tree.AddChild('d', 'x');
            var deepest = tree.GetDeepest('a');

            // Act
            var commonAncestorNode = tree.GetCommonAncestor(deepest);

            // Assert
            Assert.AreEqual('a', commonAncestorNode.Value);
        }


        // This one failed original as I had a bug with levels, which I fixed.
        [TestMethod]
        public void Tree_GetCommonAncestorOfDeepest_ShouldNotGoBackToRoot_Test()
        {
            // Arrange
            var tree = new Graph<char>();
            tree.AddChild('s', 'a');
            tree.AddChildren('a', ['b', 'c', 'd']);
            tree.AddChildren('b', ['e', 'f']);
            tree.AddChild('d', 'x');
            var deepest = tree.GetDeepest('a');

            // Act
            var commonAncestorNode = tree.GetCommonAncestor(deepest);

            // Assert
            Assert.AreEqual('a', commonAncestorNode.Value);
        }
    }
}