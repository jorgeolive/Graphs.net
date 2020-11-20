using System;

namespace Graphs.Core.Exceptions
{
    public class VertexNotFoundException : Exception
    {
        public VertexNotFoundException() : base("The vertices are not connected.") { }
    }
}
