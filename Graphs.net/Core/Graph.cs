﻿using Graphs.Core.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.Core
{
    public class Graph<T> where T : class, IEquatable<T>
    {
        public Graph(bool isWeighted, bool isDirected)
        {
            IsWeighted = isWeighted;
            IsDirected = isDirected;
        }

        public readonly bool IsWeighted;
        public readonly bool IsDirected;

        public bool IsConnected
        {
            get
            {
                if (!this.IsDirected)
                {
                    foreach (var vertex in Vertices.Values)
                    {
                        var verticesFromSource = BreadthFirstSearch.GetConnectedVertices(this, vertex);

                        if (verticesFromSource.Count() != this.Order)
                        {
                            return false;
                        }
                    }

                    return true;
                }

                throw new NotImplementedException("Directed IsConnected not implemented");
            }
        }

        public bool Cyclic => false;
        public bool IsRegular
        {
            get
            {
                if (!Vertices.Any())
                    throw new InvalidOperationException("The graph has no vertices defined.");

                bool isRegular = true;

                if (IsDirected)
                {
                    var expectedOrder = Vertices.Values.First().AdjacentEdges.Count();

                    foreach (var edge in Vertices.Values)
                    {
                        if (isRegular)
                        {
                            isRegular =
                                edge.AdjacentEdges.Count(x => x.From == edge) == edge.AdjacentEdges.Count(x => x.From == edge) &&
                                edge.AdjacentEdges.Count(x => x.From == edge) + edge.AdjacentEdges.Count(x => x.From == edge) == expectedOrder;
                        }
                    }

                }
                else
                {
                    var expectedOrder = Vertices.Values.First().Degree;
                    isRegular = Vertices.Values.All(x => x.Degree == expectedOrder);
                }

                return isRegular;
            }
        }

        public bool IsComplete => false;
        public bool IsBipartite => false;
        public int Order => Vertices.Count();
        public int Size => _edges.Count();
        public Dictionary<T, Vertex<T>> Vertices { get; } = new Dictionary<T, Vertex<T>>();
        public Dictionary<T, ICollection<Vertex<T>>> AdjancecyList { get; } = new Dictionary<T, ICollection<Vertex<T>>>();

        public bool AreVerticesConnected(T from, T to)
        {
            if (TryGetVertex(to, out Vertex<T> toVertex) && TryGetVertex(from, out Vertex<T> fromVertex))
            {
                var verticesFromSource = BreadthFirstSearch.GetConnectedVertices(this, fromVertex);
                return verticesFromSource.Contains(toVertex);
            }

            throw new ArgumentOutOfRangeException("Some vertex is not part of the graph.");
        }

        private ICollection<Edge<T>> _edges = new List<Edge<T>>();
        public bool TryGetVertex(T value, out Vertex<T> vertex) => Vertices.TryGetValue(value, out vertex);
        public void AddVertex(T @object)
        {
            if (Vertices.ContainsKey(@object))
            {
                throw new InvalidOperationException("Can't add the same vertex twice to the graph.");
            }

            var newVertex = new Vertex<T>(@object);

            Vertices.Add(@object, newVertex);
            AdjancecyList.Add(@object, new List<Vertex<T>>());
        }
        public void ConnectVertices(T from, T to)
        {
            if (IsWeighted)
                throw new InvalidOperationException("Cannot connect directed or/and weighted graphs without direction or/and weight information");

            this.ConnectVertices(from, to, new NoWeight());
        }
        public void ConnectVertices(T from, T to, Weight weight)
        {
            if (IsWeighted)
            {
                if (weight.GetType() == typeof(NoWeight))
                {
                    throw new InvalidOperationException("The graph is weighted and it needs weight information.");
                }
            }
            else
            {
                if (weight.GetType() != typeof(NoWeight))
                {
                    throw new InvalidOperationException("Can't add a weighted edge to an unweighted graph.");
                }
            }

            if (!this.Vertices.TryGetValue(from, out Vertex<T> leftVertex) || !this.Vertices.TryGetValue(to, out Vertex<T> rightVertex))
                throw new InvalidOperationException("Some of the vertices are not associated to the graph.");

            var edge = new Edge<T>(leftVertex, rightVertex, IsDirected, weight);

            _edges.Add(edge);
            leftVertex.AddEdge(edge);
            rightVertex.AddEdge(edge);
            UpdateAdjancecyListOnAdd(from, to);

        }
        private void UpdateAdjancecyListOnAdd(T from, T to)
        {
            if (AdjancecyList.TryGetValue(from, out ICollection<Vertex<T>> neightbours) && this.Vertices.TryGetValue(to, out Vertex<T> value))
            {
                neightbours.Add(value);
            }

            if (!IsDirected && AdjancecyList.TryGetValue(to, out neightbours) && this.Vertices.TryGetValue(from, out value))
            {
                neightbours.Add(value);
            }
        }
    }
}