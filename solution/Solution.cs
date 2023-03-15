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
            //Ein Produkt kann nur aus einem Lager versendet werden, in dem es noch gelagert ist.
            
            //Wenn es versendet wird, verringert sich die verfügbare Anzahl des Produktes im Lager um 1.

            //Eine bestellte Auftragszeile darf nur einmal an den Kunden geliefert werden.

            //Nur bestellte Zeilen können aus einem Lager versendet werden.

            foreach (OrderLine order in input.GetOrderLines())
            {
                Warehouse closest = null;

                foreach (Warehouse wh in input.GetWarehouses())
                {
                    if (wh.HasStock(order.Product))
                    {
                        closest = wh;
                        break;
                    }
                }

                if (closest == null)
                {
                    return;
                }

                foreach (Warehouse wh in input.GetWarehouses())
                {
                    if (wh.HasStock(order.Product))
                    {
                        double currDist = order.Customer.Position.CalculateDistance(wh.Position);
                        if (currDist < closest.Position.CalculateDistance(wh.Position))
                        {
                            closest = wh;
                        }
                    }
                }
                operations.Ship(order, closest);

            }

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
