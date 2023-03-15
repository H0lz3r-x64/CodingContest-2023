using com.knapp.CodingContest.data;
using com.knapp.CodingContest.operations;
using System.Collections.Generic;


namespace com.knapp.CodingContest.core
{
    internal class MyInputData : InputData
    {
        private readonly Dictionary<string, MyProduct> myProducts= new Dictionary<string, MyProduct>();
        private readonly Dictionary<string, MyWarehouse> myWarehouses = new Dictionary<string, MyWarehouse>();
        private readonly Dictionary<string, MyCustomer> myCustomers = new Dictionary<string, MyCustomer>();
        private readonly List<MyOrderLine> myOrderLines = new List<MyOrderLine>();

        public readonly List<WarehouseOperation> results = new();

        private readonly MyOperations operations;

        internal MyInputData( CostFactors costs )
        {
            operations = new MyOperations( this, costs) ;
        }

        internal IEnumerable<WarehouseOperation> GetResults() => results;

        internal int CountWarehouses() => myWarehouses.Count;

        internal int CountOrderLines() => myOrderLines.Count;

        internal int CountCustomers() => myCustomers.Count;

        internal int CountProducts() => myProducts.Count;

        internal MyWarehouse GetWarehouse(data.Warehouse warehouse) => myWarehouses[warehouse.Code];
        internal MyWarehouse GetWarehouse(string code) => myWarehouses[code];

        internal MyCustomer GetCustomer(data.Customer customer) => myCustomers[customer.Code];
        internal MyCustomer GetCustomer(string code) => myCustomers[code];


        internal MyProduct GetProduct(string code) => myProducts[code];

        internal IEnumerable<WarehouseOperation> GetResult() => results;


        internal void AddResult(WarehouseOperation operation) => results.Add(operation);

        public MyOperations GetOperations() => operations; 

        protected override Product NewProduct(string code, int size)
        {
            var p = new MyProduct(code, size);
            myProducts.Add(code, p);

            return p;
        }

        internal double CostUnfinishedOrderLines(int unfinishedOrderLineCount)
        {
            return (unfinishedOrderLineCount == 0 ? 0 : operations.Costs.UnfinishedOrderLinesPenalty  +
                                                        ( operations.Costs.UnfinishedOrderLinesCost * unfinishedOrderLineCount )
                );
        }

        protected override Warehouse NewWarehouse(string code, Position position)
        {
            var w = new MyWarehouse(this, code, position);
            myWarehouses.Add(code, w);

            return w;
        }

        protected override void AddWarehouseStock(Warehouse warehouse, Product product, int stock)
        {
            myWarehouses[warehouse.Code].AddStock(product.Code, stock);
            myProducts[product.Code].AddStock(stock);
        }

        public override Customer NewCustomer(string code, Position position)
        {
            var c = new MyCustomer( code, position );
            myCustomers.Add(code, c);

            return c;
        }

        public override OrderLine NewOrderLine(Customer customer, Product product)
        {
            var n = new MyOrderLine(customer, product);
            myOrderLines.Add(n);
            myCustomers[customer.Code].AddRequest( product );
            myProducts[product.Code].AddRequestedQuantity();
            return n;
        }
    }
}
