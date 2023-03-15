using System;

namespace com.knapp.CodingContest.operations.exceptions
{
    public class NoStockInWarehouseException : Exception
    {
        public NoStockInWarehouseException(string message )
            : base( message )
        {}
    }
}
