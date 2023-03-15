
namespace com.knapp.CodingContest.core
{
    internal class InputStat
    {
        public readonly int countWarehouses;
        public readonly int countCustomers;
        public readonly int countOrderLines;
        public readonly int countUniqueProducts;

        public readonly double avgOrderLinesPerCustomer;

        internal InputStat(MyInputData input)
        {
            countWarehouses = input.CountWarehouses();
            countCustomers = input.CountCustomers();
            countOrderLines = input.CountOrderLines();
            countUniqueProducts = input.CountProducts();
            avgOrderLinesPerCustomer = (double)countOrderLines / countCustomers;
        }

    }
}
