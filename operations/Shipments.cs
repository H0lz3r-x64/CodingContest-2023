using System.Collections.Generic;
using System.Linq;

namespace com.knapp.CodingContest.operations
{
    internal class Shipment
    {
        public int Count { get; private set; }

        public int Size { get; private set; }

        public readonly Dictionary<string, long> Products;

        public Shipment( List<core.ShippedProduct> shippedProducts)
        {
            Count = shippedProducts.Count;
            Size = shippedProducts.Sum(p => p.Product.Size);
            Products = shippedProducts.GroupBy(r => r.Product.Code)
                                        .Select( e => new { K = e.Key, C = e.Count() })
                                        .ToDictionary(p => p.K, p => (long)p.C);
        }

        public override string ToString() => $"{nameof(Shipment)}[#{Products.Count}/ {Count}: {Size} ]";
    }
}
