using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.data
{
    public class Customer
    {
        /// <summary>
        /// The code for this customer
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// The position of the customer - to be able to calculate distances
        /// </summary>
        public Position Position { get; private set; }

        protected Customer( string code, Position position )
        {
            Code = code;
            Position = position;
        }

        public override string ToString()
        {
            return $"Customer#{Code} [{Position}]";
        }


        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Customer other &&
                    other.Code == Code;

        }
    }
}
