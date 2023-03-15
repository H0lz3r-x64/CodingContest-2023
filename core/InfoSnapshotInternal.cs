using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.core
{
    internal class InfoSnapshotInternal : operations.IInfoSnapshot
    {
        //shipments: customer, warehouse, List<Shipments>
        private readonly Dictionary<string, Dictionary<string, operations.Shipment>> shipments = new Dictionary<string, Dictionary<string, operations.Shipment>>();

        public long TotalShipmentCount { get; private set; }
        
        public double TotalSizeByDistance { get; private set; }

        public double TotalDistance { get; private set; }

        public double TotalSize { get; private set; }

        public int UnfinishedOrderLineCount { get; private set; }

        public double UnfinishedOrderLinesCost { get; private set; }
        public double ShipmentsBaseCost { get; private set; }

        public double ShipmentsSizeDistCost { get; private set; }

        public double ShipmentsCosts => ShipmentsBaseCost + ShipmentsSizeDistCost;

        public double TotalCost { get; private set; }

        internal InfoSnapshotInternal(MyOperations operations)
        {
            MyInputData input = operations.GetInput();

            List<ShippedProduct> shippedProducts = new List<ShippedProduct>();

            foreach (var r in operations.GetResults())
            {
                if (r is operations.Ship ship)
                {
                    shippedProducts.Add(new ShippedProduct(ship, input));
                }
            }

            StoreShipments(shippedProducts);


            TotalShipmentCount = shippedProducts.GroupBy(p => new { p.Customer, p.Warehouse }).Distinct().Count();
            TotalSizeByDistance = 0;
            foreach( var s in shippedProducts.GroupBy(p => new { p.Customer, p.Warehouse }) )
            {
                var distance = s.Key.Customer.Position.CalculateDistance(s.Key.Warehouse.Position);
                var increment = s.Sum(p => p.Product.Size * distance);

                TotalSizeByDistance += increment;
            }

            TotalDistance = shippedProducts.GroupBy(p => new { p.Customer, p.Warehouse }).Sum(x => x.Key.Customer.Position.CalculateDistance(x.Key.Warehouse.Position)); ;
            TotalSize = shippedProducts.Sum(p => p.Product.Size);

            UnfinishedOrderLineCount = input.GetOrderLines().Count() - shippedProducts.Count();

            UnfinishedOrderLinesCost =  input.CostUnfinishedOrderLines(UnfinishedOrderLineCount);

            ShipmentsBaseCost = shippedProducts.GroupBy( p => new { p.Customer, p.Warehouse }).Count() * operations.Costs.ShipmentBaseCosts;

            ShipmentsSizeDistCost = shippedProducts.GroupBy(p => new { p.Customer, p.Warehouse })
                                        .Select(p => new { Warehouse = p.Key.Warehouse, Customer = p.Key.Customer, Products = p.Select( x => x.Product ).Cast<data.Product>().ToList() })
                                        .Sum(p => operations.CostSingleShipment(p.Warehouse, p.Customer, p.Products) )
                                        - ShipmentsBaseCost;



            TotalCost = UnfinishedOrderLinesCost + ShipmentsBaseCost + ShipmentsSizeDistCost;



        }


        private void StoreShipments( List<ShippedProduct> shippedProducts )
        {
            // List<Tuple<MyWarehouse, Dictionary<MyCustomer, List<ShippedProduct>>>> 
            var wcps = shippedProducts
                         .GroupBy(p => p.Customer);

            foreach( var c in shippedProducts.GroupBy( p => p.Customer ) )
            {
                MyCustomer customer = c.Key;

                foreach( var w in c.GroupBy( p=> p.Warehouse ) )
                {
                    MyWarehouse warehouse = w.Key;

                    foreach (var p in w.GroupBy(p => p.Product))
                    {
                        var s = new operations.Shipment(p.ToList());

                        //shipments: customer, warehouse, List<Shipments>
                        if (!shipments.ContainsKey(customer.Code))
                        {
                            shipments.Add(customer.Code, new Dictionary<string, operations.Shipment>());
                        }

                        if( shipments[customer.Code ].ContainsKey( warehouse.Code ) )
                        {
                            shipments[customer.Code][warehouse.Code] = s;

                        }
                        else
                        {
                                shipments[customer.Code].Add(warehouse.Code, s);
                        }
                    }
                }
            }
        }
        public override string ToString()
        {
             StringBuilder sb = new StringBuilder();
            sb.Append("InfoSnapshotInternal[")
                .Append("totalShipmentCount=")
                .Append(TotalShipmentCount)
                .Append(", totalDistance=")
                .Append(TotalDistance)
                .Append(", totalSize=")
                .Append(TotalSize)

                .Append(", shipments=")
                .Append(shipments)
                .Append(", unfinishedOrderLineCount=")
                .Append(UnfinishedOrderLineCount)

                .Append(", unfinishedOrderLinesCost=")
                .Append(UnfinishedOrderLinesCost)

                .Append(", shipmentsBaseCost=")
                .Append(ShipmentsBaseCost)
                .Append(", shipmentsSizeDistCost=")
                .Append(ShipmentsSizeDistCost)
                .Append(", totalCost=")
                .Append(TotalCost)
                .Append("]");
            return sb.ToString();
        }
    }
}
