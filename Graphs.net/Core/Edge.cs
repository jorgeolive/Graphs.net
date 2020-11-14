using Graphs.Core.Enum;
using System;

namespace Graphs.Core
{
    public sealed record Edge<T> : IEquatable<Edge<T>> where T : class, IEquatable<T>
    {
        public Edge(Vertex<T> left, Vertex<T> right, Direction direction, Weight weight)
        => (LeftIncident, RightIncident, Direction, Weight) = (left, right, direction, weight);

        public bool IsWeighted() => Weight.GetType() == typeof(NoWeight);
        public bool IsDirected() => Direction == Direction.NotDirected;
        public Vertex<T> LeftIncident;
        public Vertex<T> RightIncident;
        public Weight Weight;
        public Direction Direction;

        public bool Equals(Edge<T> other)
        {
            if (this.Direction == Direction.NotDirected)
            {
                if (other.Direction != Direction.NotDirected)
                    throw new InvalidOperationException("There's inconsistent edge data in the graph: Directed edges coexisting with undirected");

                if (this.LeftIncident.Value == other.LeftIncident.Value && this.RightIncident.Value == other.RightIncident.Value)
                    return true;

                if (this.LeftIncident.Value == other.RightIncident.Value && this.RightIncident.Value == other.LeftIncident.Value)
                    return true;
            }
            else
            {
                if (other.Direction == Direction.NotDirected)
                    throw new InvalidOperationException("There's inconsistent edge data in the graph: Directed edges coexisting with undirected");

                if (this.LeftIncident.Value == other.LeftIncident.Value && this.RightIncident.Value == other.RightIncident.Value && this.Direction == other.Direction)
                    return true;

                if (this.LeftIncident.Value == other.RightIncident.Value && this.RightIncident.Value == other.LeftIncident.Value && this.Direction == DirectionExtensions.GetOpposedTo(other.Direction))
                    return true;
            }

            return false;
        }
            
        public bool Contains(T value)
        => LeftIncident.Value == value || RightIncident.Value == value ? true : false;
    }
}