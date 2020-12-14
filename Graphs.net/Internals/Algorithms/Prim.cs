using Graphs.Core.Internals.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.Core.Internals.Algorithms
{
    internal static class Prim
    {
        public static Graph<T> CreateMinimumSpanningTreeFor<T>(Graph<T> graph) where T : class, IEquatable<T>
        {
            var visitedVertices = new HashSet<Vertex<T>>();

            if (!graph.IsConnected || graph.IsDirected)
                throw new InvalidOperationException("The graph doesn't meet the characteristics to be used with Prim algorythm.");

            var minBinaryHeap = new MinBinaryHeap<T>(graph, graph.Vertices.Values.First());

            while (!minBinaryHeap.IsEmpty())
            {
                var vertexOnAnalysis = minBinaryHeap.ExtractFromTop();

                foreach (var edge in vertexOnAnalysis.AdjacentEdges.Where(x => !visitedVertices.Contains(x.To) && !visitedVertices.Contains(x.From)))
                {
                    var other = edge.To == vertexOnAnalysis ? edge.From : edge.To;

                    if (minBinaryHeap.Contains(other, out float? value))
                    {
                        if (value.Value > edge.Weight.Value)
                        {
                            minBinaryHeap.ReplaceValue(other, edge.Weight.Value, edge);
                        }
                    }
                }

                visitedVertices.Add(vertexOnAnalysis);
            }

            return BuildNewGraph(graph, minBinaryHeap);
        }

        private static Graph<T> BuildNewGraph<T>(Graph<T> graph, MinBinaryHeap<T> minBinaryHeap) where T : class, IEquatable<T>
        {
            var newGraph = new Graph<T>(graph.IsWeighted, graph.IsDirected);

            foreach(var vertex in graph.Vertices.Values.Select(x => x with { AdjacentEdges = null }))
            {
                newGraph.AddVertex(vertex.Value);
            }

            foreach (var edge in minBinaryHeap.GetEdges())
            {
                newGraph.ConnectVertices(edge.From.Value, edge.To.Value, edge.Weight);
            }

            return newGraph;
        }
    }
}
