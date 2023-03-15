
using com.knapp.CodingContest.data;
using com.knapp.CodingContest.operations;
using com.knapp.CodingContest.operations.exceptions;
using System.Collections.Generic;
using System.Linq;

namespace com.knapp.CodingContest.core
{
    internal class MyOperations : IOperations
    {
        private readonly MyInputData input;
        private readonly CostFactors costs;

        public static readonly string[] COST_EVAL_OPEN_LINES =
            { "no open lines => no penalty",
              "(cost_unfinished_penalty + (unfinished_line_count * cost_unfinished_line))",
            };

        public static readonly string COST_EVAL_SHIPMENT = "(cost_base + (sum(product_size) * distance_warehouse_customer) * cost_size)";

        public CostFactors Costs => costs;
        internal IEnumerable<WarehouseOperation> GetResults() => input.GetResults();
        
        public IInfoSnapshot GetInfoSnapshot() => new InfoSnapshotInternal(this);

        internal MyOperations( MyInputData input, CostFactors costs )
        {
            this.input = input;
            this.costs = costs;
        }

        public double CostUnfinishedOrderLines(int unfinishedOrderLineCount) =>(unfinishedOrderLineCount == 0) ? 0
                                                                                : (costs.UnfinishedOrderLinesPenalty
                                                                                + (costs.UnfinishedOrderLinesCost * unfinishedOrderLineCount));

        /// <summary>
        /// Calculate the costs for a shipment.
        ///
        /// Mostly useful to build an algorithm to create all shipments before actually shipping them, while keeping actual costs low.
        /// Good results may be achieved without the need to calculate costs, by reducing #shippings, distances, ..
        ///
        ///  Note: this does NOT do any checks - be it available stock, or even if product (still) has to be shipped for customer
        /// </summary>
        /// <returns>the cost of a single shipment</returns>
        public double CostSingleShipment(Warehouse wh, Customer customer, List<Product> products) => CostSingleShipment( wh, customer, products, Costs );


        public void Ship(OrderLine orderLine, Warehouse warehouse)
        {
            if( ! warehouse.HasStock( orderLine.Product) )
            {
                throw new NoStockInWarehouseException($"no (more) stock for {orderLine.Customer.Code} <{orderLine.Product.Code}> in <{warehouse.Code}>");
            }

            if( ! Shipped( orderLine, warehouse ) )
            {
                throw new OrderAlreadyPackedException($"no more open order-lines for {orderLine.Customer.Code}: <{orderLine.Product.Code}>");
            }

            input.AddResult(new Ship( warehouse, orderLine ));
        }

        internal MyInputData GetInput()
        {
            return input;
        }

        private bool Shipped( OrderLine orderLine, Warehouse warehouse)
        {
            var myWarehouse = input.GetWarehouse(warehouse);
            var stw = myWarehouse.GetOnHandQuantity(orderLine.Product);

            var myCustomer = input.GetCustomer( orderLine.Customer );
            var stc = myCustomer.GetShippedQuantity( orderLine.Product);

            if( stc == 0 )
            {
                return false;
            }

            myWarehouse.ShipOne( orderLine.Product );
            myCustomer.ShipOne(orderLine.Product);
            return true;
        }

        private double CostSingleShipment(Warehouse wh, Customer customer, List<Product> products, CostFactors costs )
        {
            var sumProductSize = products.Sum(p => p.Size );

            var costSize = costs.ShipmentCostsPerSizeAndDistance;

            var distanceWarehouseCustomer = wh.Position.CalculateDistance( customer.Position );

            return (costs.ShipmentBaseCosts + ((sumProductSize * distanceWarehouseCustomer) * costSize));
        }
    }
}
