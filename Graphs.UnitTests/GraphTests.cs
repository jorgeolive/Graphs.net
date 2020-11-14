using FluentAssertions;
using Graphs.Core;
using System;
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
                Name = name;
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
    }
}
