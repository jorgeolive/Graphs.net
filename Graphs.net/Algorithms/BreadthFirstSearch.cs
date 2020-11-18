using System;
using System.Collections.Generic;

namespace Graphs.Core.Algorithms
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
    }
}
