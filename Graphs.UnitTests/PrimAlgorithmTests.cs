using FluentAssertions;
using Graphs.Core;
using Xunit;
using static Graphs.UnitTests.GraphTests;

namespace Graphs.UnitTests
{
    public class PrimAlgorithmTests
    {
        [Fact]
        public void CanGenerateMinimumSpanningTree()
        {
            var graph = new Graph<City>(isWeighted: true, isDirected: false);

            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");
            var city3 = new City("28210", "Majadahonda");
            var city4 = new City("28200", "Rozas");
            var city5 = new City("28230", "Villalba");
            var city6 = new City("28240", "Moralzarzal");
            var city7 = new City("28250", "Cercedilla");

            graph.AddVertex(city1);
            graph.AddVertex(city2);
            graph.AddVertex(city3);
            graph.AddVertex(city4);
            graph.AddVertex(city5);
            graph.AddVertex(city6);
            graph.AddVertex(city7);

            graph.ConnectVertices(city1, city2, new Weight(5));
            graph.ConnectVertices(city2, city3, new Weight(2));
            graph.ConnectVertices(city3, city4, new Weight(3));
            graph.ConnectVertices(city4, city5, new Weight(4));
            graph.ConnectVertices(city5, city6, new Weight(6));
            graph.ConnectVertices(city6, city7, new Weight(7));
            graph.ConnectVertices(city7, city1, new Weight(10));
            graph.ConnectVertices(city1, city4, new Weight(1));
            graph.ConnectVertices(city1, city5, new Weight(1));
            graph.ConnectVertices(city1, city3, new Weight(1));
            graph.ConnectVertices(city5, city7, new Weight(1));


            var mst = graph.CreateSMT();
            mst.Size.Should().Be(6);
        }
    }
}
