using System;

namespace Graphs.UnitTests
{
    public partial class GraphTests
    {
        public class City : IEquatable<City>
        {
            public string PostalCode;
            public string Name;

            public City(string postalCode, string name)
            {
                PostalCode = postalCode;
                Name = name ;
            }

            public bool Equals(City other)
            => this.PostalCode == other.PostalCode;

            public override int GetHashCode() => (PostalCode, Name).GetHashCode();

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
