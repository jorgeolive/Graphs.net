using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs.Core.Internals.DataStructures
{
    internal class MinBinaryHeap<T> where T : class, IEquatable<T>
    {
        readonly int _maxSize;
        private Vertex<T>[] _objects;
        private float?[] _weights;
        private Dictionary<Vertex<T>, Edge<T>> _extendedTable = new Dictionary<Vertex<T>, Edge<T>>();
        public MinBinaryHeap(Graph<T> graph, Vertex<T> startVertex)
        {
            _maxSize = graph.Order;
            _objects = new Vertex<T>[graph.Order];
            _weights = new float?[graph.Order];

            InitializeHeap(graph, startVertex);
        }

        private void InitializeHeap(Graph<T> graph, Vertex<T> startVertex)
        {
            var tempIndex = 0;

            foreach (var vertex in graph.Vertices.Values)
            {
                _objects[tempIndex] = vertex;
                tempIndex++;
            }

            for (int i = 0; i < graph.Order; i++)
            {
                _weights[i] = float.PositiveInfinity;
            }

            SetStartVertex(startVertex);
        }

        public IEnumerable<Edge<T>> GetEdges()
        {
            return _extendedTable.Values.Select(x => x with { });
        }

        private void SetStartVertex(Vertex<T> vertex)
        {
            var found = false;
            var index = 0;

            while (!found || index < _maxSize)
            {
                if (_objects[index] == vertex)
                {
                    (_objects[0], _objects[index]) = (_objects[index], _objects[0]);
                    _weights[0] = 0;
                    found = true;
                }

                index++;
            }
        }

        public Vertex<T> ExtractFromTop()
        {
            var topElement = _objects[0];

            var lastNullPosition = Array.IndexOf(_objects, null);

            if (lastNullPosition != -1)
            {
                (_objects[0], _objects[lastNullPosition - 1]) = (_objects[lastNullPosition - 1], null);
                (_weights[0], _weights[lastNullPosition - 1]) = (_weights[lastNullPosition - 1], null);
            }
            else
            {
                (_objects[0], _objects[_maxSize - 1]) = (_objects[_maxSize - 1], null);
                (_weights[0], _weights[_maxSize - 1]) = (_weights[_maxSize - 1], null);
            }

            var continueSwapping = true;
            var index = 0;

            while (continueSwapping && index * 2 + 2 < _objects.Length)
            {
                continueSwapping = false;

                if (LeftDescendantSmallerThanElement(index))
                {
                    SwapWithLeftChild(index);
                    continueSwapping = true;
                }

                index = index * 2 + 1;
            }

            return topElement;
        }

        private bool DescendantsSmallerThanElement(int index) => LeftDescendantSmallerThanElement(index) || RightDescendantSmallerThanElement(index);
        private bool LeftDescendantSmallerThanElement(int index) => index * 2 + 1 < _maxSize &&
            _weights[index * 2 + 1] != null && _weights[index * 2 + 1] < _weights[index] && _weights[index * 2 + 2] != null && _weights[index * 2 + 2] < _weights[index];

        private bool RightDescendantSmallerThanElement(int index) => index * 2 + 2 < _maxSize &&
            _weights[index * 2 + 2] != null && _weights[index * 2 + 2] < _weights[index] && _weights[index * 2 + 2] != null && _weights[index * 2 + 2] < _weights[index];

        private bool AscendantSmallerThanElement(int index) =>
           _weights[GetParentIndexFrom(index)] != null && _weights[GetParentIndexFrom(index)] < _weights[index];

        public bool Contains(Vertex<T> vertex, out float? weight)
        {
            if (_objects.Contains(vertex))
            {
                weight = _weights[Array.IndexOf(_objects, vertex)];
                return true;
            }

            weight = null;
            return false;
        }

        private int GetParentIndexFrom(int index)
        {
            if (index == 0)
                return 0;

            decimal parentIndex = (index - 1) / 2;
            return (int)Math.Floor(parentIndex);
        }

        public void ReplaceValue(Vertex<T> vertex, float newValue, Edge<T> edge)
        {
            var position = Array.IndexOf(_objects, vertex);
            _weights[position] = newValue;
            _extendedTable[vertex] = edge;

            var keepChecking = true;
            while (keepChecking)
            {
                keepChecking = false;

                if (DescendantsSmallerThanElement(position))
                {
                    if (LeftDescendantSmallerThanElement(Array.IndexOf(_objects, vertex)))
                    {
                        SwapWithLeftChild(position);
                        position = position * 2 + 1;
                        keepChecking = true;
                    }
                    else
                    {
                        SwapWithRightChild(position);
                        position = position * 2 + 2;
                        keepChecking = true;
                    }
                }
                else if (position != 0 && !AscendantSmallerThanElement(position))
                {
                    SwapWithParent(position);
                    position = GetParentIndexFrom(position);
                    keepChecking = true;
                }
            }
        }

        private void SwapWithLeftChild(int index)
        {
            (_objects[index], _objects[index * 2 + 1]) = (_objects[index * 2 + 1], _objects[index]);
            (_weights[index], _weights[index * 2 + 1]) = (_weights[index * 2 + 1], _weights[index]);
        }

        private void SwapWithParent(int index)
        {
            var parentIndex = GetParentIndexFrom(index);

            (_objects[index], _objects[parentIndex]) = (_objects[parentIndex], _objects[index]);
            (_weights[index], _weights[parentIndex]) = (_weights[parentIndex], _weights[index]);
        }

        private void SwapWithRightChild(int index)
        {
            (_objects[index], _objects[index * 2 + 2]) = (_objects[index * 2 + 2], _objects[index]);
            (_weights[index], _weights[index * 2 + 2]) = (_weights[index * 2 + 2], _weights[index]);
        }

        public bool IsEmpty()
        {
            return _objects.Count(x => x is null) == _maxSize;
        }
    }

}
