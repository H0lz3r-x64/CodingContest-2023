using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.core
{
    internal class MyCustomer : data.Customer
    {
        private readonly Dictionary<string, int> products = new Dictionary<string, int>();

        public MyCustomer(string code, data.Position position)
            : base(code, position)
        { }

        public override string ToString() => $"MyCustomer[Code={Code}, Position={Position}]";

        internal int GetShippedQuantity(Product product)
        {
            if( ! products.ContainsKey( product.Code ) )
            {
                return 0;
            }
            
            return products[product.Code];
        }
        internal void ShipOne(Product product)
        {
            if( ! products.ContainsKey( product.Code ) )
            {
                throw new Exception($"??? Trying to ship something to a customer that does not want it: Product {product.Code} to {Code}");
            }

            products[product.Code]--;
        }

        internal void AddRequest(Product product)
        {
            if (!products.ContainsKey(product.Code))
            {
                products.Add(product.Code, 1);
            }
            else
            {
                products[product.Code]++;
            }
        }
    }
}
