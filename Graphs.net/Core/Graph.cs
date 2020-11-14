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
        public bool IsCyclic() => false;
        public bool IsRegular()
        {
            if (!_vertices.Any())
                throw new InvalidOperationException("The graph has no vertices defined.");

            bool isRegular = true;

            if (IsDirected)
            {
                var expectedOrder = _vertices.Values.First().AdjacentEdges.Count();

                foreach (var edge in _vertices.Values)
                {
                    if (isRegular)
                    {
                        isRegular = 
                            edge.AdjacentEdges.Count(x => x.From == edge) == edge.AdjacentEdges.Count(x => x.From == edge) &&
                            edge.AdjacentEdges.Count(x => x.From == edge) + edge.AdjacentEdges.Count(x => x.From == edge) == expectedOrder;
                    }
                }

            } else {
                var expectedOrder = _vertices.Values.First().Degree;
                isRegular = _vertices.Values.All(x => x.Degree == expectedOrder);
            }

            return isRegular;
        }
        public bool IsComplete() => false;
        public bool IsBipartite() => false;

        public int Order => _vertices.Count();
        public int Size => _edges.Count();

        private Dictionary<T, Vertex<T>> _vertices = new Dictionary<T, Vertex<T>>();
        public ICollection<Edge<T>> _edges = new List<Edge<T>>();

        public IEnumerable<Vertex<T>> GetVertices() => _vertices.Values;
        public IEnumerable<Edge<T>> GetAdjacencyList() => _edges;

        public void AddVertex(T @object)
        {
            if (_vertices.ContainsKey(@object))
            {
                throw new InvalidOperationException("Can't add the same vertex twice to the graph.");
            }

            _vertices.Add(@object, new Vertex<T>(@object));
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

            if (!this._vertices.TryGetValue(from, out Vertex<T> leftVertex) || !this._vertices.TryGetValue(to, out Vertex<T> rightVertex))
                throw new InvalidOperationException("Some of the vertices are not associated to the graph.");

            var edge = new Edge<T>(leftVertex, rightVertex, IsDirected, weight);

            _edges.Add(edge);
            leftVertex.AddEdge(edge);
            rightVertex.AddEdge(edge);
        }
    }
}