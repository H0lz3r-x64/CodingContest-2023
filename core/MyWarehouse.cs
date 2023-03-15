using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.core
{
    internal class MyWarehouse : data.Warehouse
    {
        private readonly Dictionary<string, int> productStock = new Dictionary<string, int>();

        private readonly MyInputData input;

        internal MyWarehouse(MyInputData input, string code, Position position)
            : base(code, position)
        {
            this.input = input;
        }


        public override bool HasStock(Product product) => productStock.ContainsKey(product.Code) && productStock[product.Code] > 0;

        public override IDictionary<Product, int> GetCurrentStock()
        {
            Dictionary<Product, int> result = new Dictionary<Product, int>();

            foreach (var p in productStock)
            {
                result.Add( input.GetProduct(p.Key), p.Value);
            }

            return result;
        }
        internal int GetOnHandQuantity(Product p) => productStock[p.Code];

        internal void ShipOne(Product p)
        {
            productStock[p.Code]--;
        }

        internal void AddStock(string code, int stock)
        {
            if (!productStock.ContainsKey(code))
            {
                productStock.Add(code, stock);
            }
            else
            {
                productStock[code] += stock;
            }
        }
    }
}
