using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.Core
{
    public sealed record Vertex<T> : IEquatable<Vertex<T>> where T : class, IEquatable<T>
    {
        public Vertex(T value) => Value = value;
        public T Value;
        public int Degree => _adjacentEdges.Count();

        private ICollection<Edge<T>> _adjacentEdges = new List<Edge<T>>();

        public bool Equals(Vertex<T> other) => other.Value.Equals(this.Value);

        public void AddEdge(Edge<T> edge)
        {
            if (_adjacentEdges.Any(x => x == edge))
                throw new InvalidOperationException("There's already and edge joining vertices with that direction.");

            if (edge.LeftIncident.Value == this.Value || edge.RightIncident.Value == this.Value)
            {
                _adjacentEdges.Add(edge);
            } else
            {
                throw new InvalidOperationException("Neither edge value matches to the vertex value.");
            }
        }
    }
}