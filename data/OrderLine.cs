using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.data
{
    public class OrderLine
    {
        /// <summary>
        /// The product to be shipped
        /// </summary>
        public Product Product { get; private set; }

        /// <summary>
        /// the customer to whom this product is to be shipped
        /// </summary>
        public Customer Customer { get; private set; }


        public OrderLine( Customer customer, Product product )
        {
            Customer = customer;
            Product = product;
        }

        public override string ToString() => $"OrderLine[Customer={Customer}, Product={Product}]";
    }
}
