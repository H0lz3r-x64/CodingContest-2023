using System;

namespace com.knapp.CodingContest.operations.exceptions
{
    public class OrderAlreadyPackedException : Exception
    {
        public OrderAlreadyPackedException(string message)
            : base(message)
        { }
    }
}
