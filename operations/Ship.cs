using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.operations
{
    internal class Ship : core.WarehouseOperation
    {
        public string WhCode { get; private set; }

        public string ProdCode { get; private set; }

        public string CustCode { get; private set; }


        public Ship(data.Warehouse warehouse, data.OrderLine orderLine )
            : base(warehouse.Code, orderLine.Product.Code, orderLine.Customer.Code )
        {
            WhCode = warehouse.Code;
            ProdCode = orderLine.Product.Code;
            CustCode = orderLine.Customer.Code;
        }
    }
}
