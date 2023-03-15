using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.core
{
    internal class MyOrderLine : data.OrderLine
    {
        internal MyOrderLine(data.Customer customer, data.Product product)
            : base(customer, product)
        { }

        public override string ToString() => $"{nameof(MyOrderLine)}[Customer={Customer.Code}, Product={Product.Code}]";
    }
}
