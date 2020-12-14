using Graphs.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace Graphs.Core.Internals.Algorithms
{
    internal static class BreadthFirstSearch
    {
        internal static HashSet<Vertex<T>> GetConnectedVertices<T>(Graph<T> graph, Vertex<T> from) where T : class, IEquatable<T>
        {
            var discoveryList = new HashSet<Vertex<T>>();
            var queue = new Queue<Vertex<T>>();

            discoveryList.Add(from);
            queue.Enqueue(from);

            while (queue.Count != 0)
            {
                if (graph.AdjancecyList.TryGetValue(queue.Dequeue().Value, out ICollection<Vertex<T>> neighbours))
                {
                    foreach (var neighbour in neighbours)
                    {
                        if (!discoveryList.Contains(neighbour))
                        {
                            discoveryList.Add(neighbour);
                            queue.Enqueue(neighbour);
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Vertex not part of the graph.");
                }
            }

            return discoveryList;
        }

        internal static IEnumerable<Vertex<T>> GetShortestPathBetween<T>(Graph<T> graph, Vertex<T> from, Vertex<T> to) where T : class, IEquatable<T>
        {
            if(graph.IsDirected || graph.IsWeighted)
            {
                throw new InvalidOperationException("BFS shortest path algorithm can only be applied on unweighted undirected graphs.");
            }

            var found = false;
            var discoveryList = new HashSet<Vertex<T>>();
            var queue = new Queue<Vertex<T>>();
            var fromMap = new Dictionary<Vertex<T>, Vertex<T>>();
            var path = new Stack<Vertex<T>>();

            discoveryList.Add(from);
            queue.Enqueue(from);

            while (queue.Count != 0 && !found)
            {
                var nextVertex = queue.Dequeue();

                if (graph.AdjancecyList.TryGetValue(nextVertex.Value, out ICollection<Vertex<T>> neighbours))
                {
                    foreach (var neighbour in neighbours)
                    {
                        if (!discoveryList.Contains(neighbour))
                        {
                            discoveryList.Add(neighbour);
                            queue.Enqueue(neighbour);
                            fromMap.Add(neighbour, nextVertex);
                        }

                        if (neighbour == to)
                        {
                            found = true;
                        }
                    }
                }

                if (found)
                {
                    path.Push(to);
                    var current = to;

                    while(fromMap.TryGetValue(current, out Vertex<T> previous))
                    {
                        path.Push(previous);
                        current = previous;                        
                    }
                }
            }

            if (!found)
            {
                throw new VerticesNotConnectedException();
            }

            return path.ToArray();
        }
    }
}
