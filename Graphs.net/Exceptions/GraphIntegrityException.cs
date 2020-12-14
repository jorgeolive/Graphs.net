using System;

namespace Graphs.Core.Exceptions
{
    public class GraphIntegrityException : Exception
    {
        public GraphIntegrityException() : base("Graph is in inconsistent state.") { }

        public GraphIntegrityException(string msg) : base(msg) { }
    }
}
