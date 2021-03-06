﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.Core
{
    public sealed record Vertex<T> : IEquatable<Vertex<T>> where T : class, IEquatable<T>
    {
        public Vertex(T value) => Value = value;
        public T Value;
        public int Degree => AdjacentEdges.Count();

        public ICollection<Edge<T>> AdjacentEdges = new List<Edge<T>>();

        public bool Equals(Vertex<T> other) => other.Value.Equals(this.Value);

        public void AddEdge(Edge<T> edge)
        {
            if (AdjacentEdges.Any(x => x == edge))
                throw new InvalidOperationException("There's already and edge joining vertices with that direction.");

            if (edge.From.Value == this.Value || edge.To.Value == this.Value)
            {
                AdjacentEdges.Add(edge);
            } else
            {
                throw new InvalidOperationException("Neither edge value matches to the vertex value.");
            }
        }
    }
}