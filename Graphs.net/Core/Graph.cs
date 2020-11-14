using Graphs.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.Core
{
    public class Graph<T> where T : class, IEquatable<T>
    {
        public Graph(bool isWeighted, bool isDirected)
        {
            _isWeighted = isWeighted;
            _isDirected = isDirected;
        }

        private bool _isWeighted;
        private bool _isDirected;
        public bool IsCyclic() => false;
        public bool IsRegular() => false;
        public bool IsComplete() => false;
        public bool IsBipartite() => false;

        public int Order => _vertices.Count();
        public int Size => _edges.Count();

        private Dictionary<T, Vertex<T>> _vertices = new Dictionary<T, Vertex<T>>();
        public ICollection<Edge<T>> _edges = new List<Edge<T>>();

        public IEnumerable<Vertex<T>> GetVertices() => _vertices.Values;
        public IEnumerable<Edge<T>> GetAdjancencyList() => _edges;

        public void AddVertex(T @object)
        {
            if (_vertices.ContainsKey(@object))
            {
                throw new InvalidOperationException("Can't add the same vertex twice to the graph.");
            }

            _vertices.Add(@object, new Vertex<T>(@object));
        }

        public void ConnectVertices(T left, T right)
        {
            if (_isWeighted || _isDirected)
                throw new InvalidOperationException("Cannot connect directed or/and weighted graphs without direction or/and weight information");

            this.ConnectVertices(left, right, Direction.NotDirected, new NoWeight());
        }

        public void ConnectVertices(T left, T right, Direction direction)
        {
            if (_isWeighted || _isDirected && direction == Direction.NotDirected)
                throw new InvalidOperationException("Cannot connect directed or/and weighted graphs without direction or/and weight information");

            this.ConnectVertices(left, right, direction, new NoWeight());
        }

        public void ConnectVertices(T left, T right, Weight weight)
        {
            if (_isDirected)
                throw new InvalidOperationException("Can't connect vertices in directed graph without direction information.");

            if (_isWeighted)
            {
                if(weight.GetType() == typeof(NoWeight))
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

            this.ConnectVertices(left, right, Direction.NotDirected, weight);
        }

        public void ConnectVertices(T left, T right, Direction direction, Weight weight)
        {
            if (weight.GetType() != typeof(NoWeight))
                _isWeighted = true;

            if (direction != Direction.NotDirected)
                _isDirected = true;

            if (!this._vertices.TryGetValue(left, out Vertex<T> leftVertex) || !this._vertices.TryGetValue(right, out Vertex<T> rightVertex))
                throw new InvalidOperationException("Some of the vertices are not associated to the graph.");

            var edge = new Edge<T>(leftVertex, rightVertex, direction, weight);

            _edges.Add(edge);
            leftVertex.AddEdge(edge);
            rightVertex.AddEdge(edge);
        }
    }
}