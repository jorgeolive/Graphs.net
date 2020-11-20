using FluentAssertions;
using Graphs.Core;
using System;
using System.Linq;
using Xunit;

namespace Graphs.UnitTests
{
    public class GraphTests
    {
        private class City : IEquatable<City>
        {
            public string PostalCode;
            public string Name;

            public City(string postalCode, string name)
            {
                PostalCode = postalCode;
                Name = name ;
            }

            public bool Equals(City other)
            => this.PostalCode == other.PostalCode;

            public override int GetHashCode() => (PostalCode, Name).GetHashCode();
        }

        [Fact]
        public void CanAddOneEdgeWithTwoVertices()
        {
            var graph = new Graph<City>(
                isWeighted: false,
                isDirected: false);

            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");

            graph.AddVertex(city1);
            graph.AddVertex(city2);

            graph.ConnectVertices(city1, city2);

            graph.Order.Should().Be(2);
            graph.Size.Should().Be(1);
        }

        [Fact]
        public void CanGetGraphSize()
        {
            var graph = new Graph<City>(
                isWeighted: false,
                isDirected: false);

            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");
            var city3 = new City("28000", "Madrid");

            graph.AddVertex(city1);
            graph.AddVertex(city2);
            graph.AddVertex(city3);

            graph.ConnectVertices(city1, city2);
            graph.ConnectVertices(city1, city3);

            graph.Size.Should().Be(2);
        }

        [Fact]
        public void CanGetGraphOrder()
        {
            var graph = new Graph<City>(
                isWeighted: false,
                isDirected: false);

            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");
            var city3 = new City("28000", "Madrid");

            graph.AddVertex(city1);
            graph.AddVertex(city2);
            graph.AddVertex(city3);

            graph.ConnectVertices(city1, city2);
            graph.ConnectVertices(city1, city3);

            graph.Order.Should().Be(3);
        }

        [Fact]
        public void CantAddWeightedEdgeToUnWeightedGraph()
        {
            var graph = new Graph<City>(isWeighted: false, isDirected: false);
            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");

            graph.AddVertex(city1);
            graph.AddVertex(city2);

            graph.Invoking(x => x.ConnectVertices(city1, city2, new Weight(10))).Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void RegularGraphIsProperlyIdentified()
        {
            var graph = new Graph<City>(isWeighted: false, isDirected: false);
            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");
            var city3 = new City("28210", "Majadahonda");
            var city4 = new City("28200", "Rozas");

            graph.AddVertex(city1);
            graph.AddVertex(city2);
            graph.AddVertex(city3);
            graph.AddVertex(city4);

            graph.ConnectVertices(city1, city2);
            graph.ConnectVertices(city2, city3);
            graph.ConnectVertices(city3, city4);
            graph.ConnectVertices(city4, city1);

            graph.IsRegular().Should().BeTrue();
        }

        [Fact]
        public void NonRegularGraphIsProperlyIdentified()
        {
            var graph = new Graph<City>(isWeighted: false, isDirected: false);
            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");
            var city3 = new City("28210", "Majadahonda");
            var city4 = new City("28200", "Rozas");

            graph.AddVertex(city1);
            graph.AddVertex(city2);
            graph.AddVertex(city3);
            graph.AddVertex(city4);

            graph.ConnectVertices(city1, city2);
            graph.ConnectVertices(city2, city3);
            graph.ConnectVertices(city3, city4);

            graph.IsRegular().Should().BeFalse();
        }

        [Fact]
        public void CantAddUnWeightedEdgeToWeightedGraph()
        {
            var graph = new Graph<City>(isWeighted: true, isDirected: false);
            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");

            graph.AddVertex(city1);
            graph.AddVertex(city2);

            graph.Invoking(x => x.ConnectVertices(city1, city2)).Should().Throw<InvalidOperationException>();
            graph.Invoking(x => x.ConnectVertices(city1, city2, new NoWeight())).Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void VerticesShouldBeConnected()
        {
            var graph = new Graph<City>(isWeighted: false, isDirected: false);
            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");
            var city3 = new City("28210", "Majadahonda");

            graph.AddVertex(city1);
            graph.AddVertex(city2);
            graph.AddVertex(city3);

            graph.ConnectVertices(city1, city2);

            graph.AreVerticesConnected(city1, city2).Should().BeTrue();
        }

        [Fact]
        public void VerticesShouldBeNotConnected()
        {
            var graph = new Graph<City>(isWeighted: false, isDirected: false);
            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");
            var city3 = new City("28210", "Majadahonda");

            graph.AddVertex(city1);
            graph.AddVertex(city2);
            graph.AddVertex(city3);

            graph.ConnectVertices(city1, city2);

            graph.AreVerticesConnected(city1, city3).Should().BeFalse();
            graph.AreVerticesConnected(city2, city3).Should().BeFalse();
        }

        [Fact]
        public void DirectedGraphVerticesShouldNotBeConnected()
        {
            var graph = new Graph<City>(isWeighted: false, isDirected: true);
            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");
            var city3 = new City("28210", "Majadahonda");

            graph.AddVertex(city1);
            graph.AddVertex(city2);
            graph.AddVertex(city3);

            graph.ConnectVertices(city1, city2);

            graph.AreVerticesConnected(city2, city1).Should().BeFalse();
        }

        [Fact]
        public void DirectedGraphVerticesShouldBeConnected()
        {
            var graph = new Graph<City>(isWeighted: false, isDirected: true);
            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");
            var city3 = new City("28210", "Majadahonda");

            graph.AddVertex(city1);
            graph.AddVertex(city2);
            graph.AddVertex(city3);

            graph.ConnectVertices(city1, city2);

            graph.AreVerticesConnected(city1, city2).Should().BeTrue();
        }

        [Fact]
        public void GraphShouldBeConnected()
        {
            var graph = new Graph<City>(isWeighted: false, isDirected: false);
            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");
            var city3 = new City("28210", "Majadahonda");

            graph.AddVertex(city1);
            graph.AddVertex(city2);
            graph.AddVertex(city3);

            graph.ConnectVertices(city1, city2);
            graph.ConnectVertices(city1, city3);

            graph.IsConnected.Should().BeTrue();
        }

        [Fact]
        public void GraphShouldNotBeConnected()
        {
            var graph = new Graph<City>(isWeighted: false, isDirected: false);
            var city1 = new City("28903", "Getafe");
            var city2 = new City("28220", "Valdemorillo");
            var city3 = new City("28210", "Majadahonda");

            graph.AddVertex(city1);
            graph.AddVertex(city2);
            graph.AddVertex(city3);

            graph.ConnectVertices(city1, city2);

            graph.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void ShortestPathIsFoundOnUnWeightedUndirectedGraph()
        {
            var graph = new Graph<City>(isWeighted: false, isDirected: false);

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

            graph.ConnectVertices(city1, city2);
            graph.ConnectVertices(city2, city3);
            graph.ConnectVertices(city3, city4);
            graph.ConnectVertices(city4, city5);
            graph.ConnectVertices(city5, city6);
            graph.ConnectVertices(city6, city7);
            graph.ConnectVertices(city7, city1);

            var shortestPath =  graph.GetShortestPathBetween(city1, city5);

            shortestPath.ElementAt(0).Value.Should().Be(city1);
            shortestPath.ElementAt(1).Value.Should().Be(city7);
            shortestPath.ElementAt(2).Value.Should().Be(city6);
            shortestPath.ElementAt(3).Value.Should().Be(city5);

        }
    }
}
