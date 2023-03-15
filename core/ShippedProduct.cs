
namespace com.knapp.CodingContest.core
{
    internal class ShippedProduct
    {
        public MyWarehouse Warehouse { get; private set; }

        public MyCustomer Customer { get; private set; }

        public MyProduct Product { get; private set; }

        internal ShippedProduct(operations.Ship ship, MyInputData input )
        {
            Warehouse = input.GetWarehouse(ship.WhCode);
            Customer = input.GetCustomer(ship.CustCode);
            Product = input.GetProduct( ship.ProdCode);
        }
    }
}
