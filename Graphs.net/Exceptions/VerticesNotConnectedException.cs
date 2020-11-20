using System;

namespace Graphs.Core.Exceptions
{
    public class VerticesNotConnectedException : Exception
    {
        public VerticesNotConnectedException(): base("The vertices are not connected.") { }
    }
}
