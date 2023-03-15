using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.data
{
    public abstract class Warehouse
    {
        /// <summary>
        /// Code fot this warehouse
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Position of this warehouse - to be able to calculate distances
        /// </summary>
        public Position Position { get; private set; }


        internal Warehouse( string code, Position position )
        {
            Code = code;
            Position = position;
        }


        /// <summary>
        /// Check to see of the warehouse has (at least) one item on stock
        /// </summary>
        /// <param name="product">the product to check</param>
        ///<returns>true when at least one piece of the product is on stock</returns>
        public abstract bool HasStock(Product product);

        /// <summary>
        /// A snapshot of the current available stock(s) for this warehouse
        /// </summary>
        /// <returns>read-only snapshot of stock(s)</returns>
        public abstract IDictionary<Product, int> GetCurrentStock();

        public override string ToString() => $"Warehouse#{Code}[{Position}]";

        public override bool Equals(object obj)
        {
            return obj is Warehouse other &&
                Code == other.Code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
