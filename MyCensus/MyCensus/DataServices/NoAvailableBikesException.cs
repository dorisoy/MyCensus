using System;

namespace MyCensus.DataServices
{
    public class NoAvailableBikesException : Exception
    {
        public NoAvailableBikesException()
        {

        }

        public NoAvailableBikesException(string message) : base(message)
        {

        }
    }
}
