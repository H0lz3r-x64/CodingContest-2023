using com.knapp.CodingContest.data;
using System.Collections.Generic;
using System.Linq;
using System;
using com.knapp.CodingContest.operations;
using com.knapp.CodingContest.core;
using com.knapp.CodingContest.operations.exceptions;

namespace com.knapp.CodingContest.solution
{
    public class Solution
    {

        public string ParticipantName { get; protected set; }

        public Institutes Institute { get; protected set; }

        protected readonly InputData input;

        protected readonly IOperations operations;

        public Solution(InputData input, IOperations operations)
        {
            this.input = input;
            this.operations = operations;


            ParticipantName = "Luca Holzer";
            Institute = Institutes._Knapp_;

            //TODO: Prepare data structures
        }

        /// <summary>
        ///   
        /// The main entry-point.
        ///
        /// calculation for shipments costs:
        ///    total_cost = sum{products per warehouse/customer } ((cost_base + (sum(size_products)* cost_size)) * distanz_warehouse_to_customer)
        ///    
        /// some hints:
        ///   - one shipments is: all products for one customer from one warehouse(will be handled and calculated automatically/internally)
        ///   - there are finite amounts of product stocks in the warehouses(stock will be adjusted automatically by using op.ship() method)
        ///   - not all warehouses have all products on stock - or stock might run out (may be checked via wh.hasStock())
        ///
        /// optimization is possible along two factors:
        ///   - minimize warehouse/customer pairs(#shipments) - reduce cost_base impact on total costs
        ///   - minimize distances - shipments from closer warehouses are cheaper
        ///
        /// some ideas for finding a better solution:
        ///   sometimes it might be beneficial to split an order to have most delivered from close warehouse and only some from farther
        /// instead of trying to deliver everything from just one warehouse that is far away
        ///
        /// </summary>
        public virtual void Run()
        {
            #region Setup
            //Sort by customers and then by Products
            List<OrderLine> orderLines = input.GetOrderLines().ToList<OrderLine>();
            orderLines = orderLines.OrderBy(x => x.Customer.Code).ThenBy(y => y.Product.Code).ToList();

            // Initialize variables
            string lastCustomerCode = orderLines[0].Customer.Code;
            string lastProductCode = orderLines[0].Product.Code;
            Warehouse nearestWh = findAnyWarehouseWithStock(orderLines[0], orderLines);
            #endregion

            int i = 0;
            foreach (OrderLine order in orderLines)
            {
                // TODO: Noch ned richtig da es derweil grob alle produkte nur zum am nähesten des ersten bringt

                // check
                if (lastCustomerCode != order.Customer.Code)
                {
                    // if calculate the nearest again
                    nearestWh = findNearestWarehouse(order, orderLines);
                }
                else if (!nearestWh.HasStock(order.Product))
                {
                    nearestWh = findNearestWarehouse(order, orderLines);
                }
                operations.Ship(order, nearestWh);
                lastCustomerCode = order.Customer.Code;
                lastProductCode = order.Product.Code;
                i++;
            }
        }

        public Warehouse findNearestWarehouse(OrderLine order, List<OrderLine> orders)
        {
            // get the nearest warehouse to the customer
            Warehouse nearestWh = findAnyWarehouseWithStock(order, orders);
            foreach (Warehouse wh in input.GetWarehouses())
            {
                if (wh.HasStock(order.Product))
                {
                    double currDist = order.Customer.Position.CalculateDistance(wh.Position);
                    if (currDist < nearestWh.Position.CalculateDistance(wh.Position))
                    {
                        nearestWh = wh;
                    }
                }
            }
            return nearestWh;
        }

        public Warehouse findAnyWarehouseWithStock(OrderLine order, List<OrderLine> orders)
        {
            Warehouse warehouse = null;
            // just set warehouse to any which has it in stock
            foreach (Warehouse wh in input.GetWarehouses())
                if (wh.HasStock(order.Product))
                {
                    warehouse = wh;
                    break;
                }
            return warehouse;
        }

        /// <summary>
        /// Just for documentation purposes.
        ///
        /// Method may be removed without any side-effects
        ///
        /// divided into 4 sections
        ///
        ///     <li><em>input methods</em>
        ///
        ///     <li><em>main interaction methods</em>
        ///         - these methods are the ones that make (explicit) changes to the warehouse
        ///
        ///     <li><em>information</em>
        ///         - information you might need for your solution
        ///
        ///     <li><em>additional information</em>
        ///         - various other infos: statistics, information about (current) costs, ...
        ///
        /// </summary>
#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
        private void Apis()
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen
        {
            // ----- input -----

            IReadOnlyList<OrderLine> orderLines = input.GetOrderLines();
            IReadOnlyList<Warehouse> warehouses = input.GetWarehouses();

            OrderLine orderLine = orderLines[0];
            Warehouse warehouse = warehouses[0];

            // ----- main interaction methods -----

            operations.Ship(orderLine, warehouse); // throws OrderLineAlreadyPackedException, NoStockInWarehouseException;

            // ----- information -----

            bool hasStock = warehouse.HasStock(orderLine.Product);
            IDictionary<Product, int> currentStocks = warehouse.GetCurrentStock();

            double distance = orderLine.Customer.Position.CalculateDistance(warehouse.Position);

            // ----- additional information -----
            CostFactors costFactors = operations.Costs;

            IInfoSnapshot info = operations.GetInfoSnapshot();
            int unfinishedOrderLineCount = info.UnfinishedOrderLineCount;
            double unfinishedOrderLinesCost = info.UnfinishedOrderLinesCost;
            double shipmentsCost = info.ShipmentsCosts;
            double totalCost = info.TotalCost;
        }
    }
}
