using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.data
{
    public class Product
    {
        /// <summary>
        /// Code for this product
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// The size of a single item of this product
        /// </summary>
        public int Size { get; private set; }

        public Product( string code, int size )
        {
            Code = code;
            Size = size;
        }

        public override string ToString() => $"Product#{Code}[size={Size}]";

        public override bool Equals(object obj)
        {
            return obj is Product other
                && Code == other.Code;
        }

        public override int GetHashCode() => Code.GetHashCode();
    }
}
