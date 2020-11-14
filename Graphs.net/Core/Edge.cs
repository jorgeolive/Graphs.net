using System;

namespace Graphs.Core
{
    //TODO : Check si aplica que el weight tipado
    public sealed record Edge<T> : IEquatable<Edge<T>> where T : class, IEquatable<T>
    {
        public Edge(Vertex<T> from, Vertex<T> to, bool isDirected, Weight weight)
        => (From, To, IsDirected, Weight) = (from, to, isDirected, weight);

        public bool IsWeighted() => Weight.GetType() == typeof(NoWeight);
        public Vertex<T> From;
        public Vertex<T> To;
        public Weight Weight;
        public bool IsDirected;

        public bool Equals(Edge<T> other)
        {
            if (!IsDirected)
            {
                if (other.IsDirected)
                    throw new InvalidOperationException("Inconsistent edge data in the graph: Directed edges coexisting with undirected");

                if (this.From.Value == other.From.Value && this.To.Value == other.To.Value)
                    return true;

                if (this.From.Value == other.To.Value && this.To.Value == other.From.Value)
                    return true;
            }
            else
            {
                if (!other.IsDirected)
                    throw new InvalidOperationException("There's inconsistent edge data in the graph: Directed edges coexisting with undirected");

                if (this.From.Value == other.From.Value && this.To.Value == other.To.Value)
                    return true;
            }

            return false;
        }
            
        public bool Contains(T value)
        => From.Value == value || To.Value == value ? true : false;
    }
}