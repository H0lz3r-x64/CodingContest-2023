using com.knapp.CodingContest.core;
using com.knapp.CodingContest.data;

namespace com.knapp.CodingContest.operations
{
    public interface IOperations
    {
        /// <summary>
        /// Ship a single product from a warehouse to a customer to fulfill an <code>OrderLine</code>.
        /// 
        ///  Validation is done to ensure stock availability and correctness of products for customer
        ///  Then internal state is adjusted accordingly: stock, customer/product, ...
        ///  
        /// </summary>
        /// <param name="orderLine">orderline to ship</param>
        /// <param name="warehouse">warehouse the warehouse to </param>
        /// <exception cref="OrderLineAlreadyPackedException"></exception>
        /// <exception cref="NoStockInWarehouseException"></exception>
        public void Ship(OrderLine orderLine, Warehouse warehouse);

        /// <summary>
        /// a snapshot of various information: costs so far, unfinished count
        /// </summary>
        /// <returns>a snapshot of various information: costs so far, unfinished count</returns>
        public IInfoSnapshot GetInfoSnapshot();

        /// <summary>
        /// the cost factors used
        /// </summary>
        public CostFactors Costs { get; }
    }
}
