
using System;

namespace com.knapp.CodingContest.core
{
    internal class MyProduct : data.Product
    {
        private int stockQuantity;
        private int requestedQuantity;

        internal MyProduct( string code, int size )
            : base( code, size )
        { }

        public override string ToString() => $"MyProduct[stockQuantity={stockQuantity}, requestedQuantity={requestedQuantity}, Code={Code}, Size={Size}]";

        internal void AddStock(int stock) => stockQuantity += stock;

        internal void AddRequestedQuantity() => requestedQuantity++;
    }
}
