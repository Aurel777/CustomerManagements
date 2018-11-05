namespace CustomerManagement.Database
{
    using System;

    public class InvalidConnectionStringException : Exception
    {
        public InvalidConnectionStringException() { }

        public InvalidConnectionStringException(string message) : base(message) { }

        public InvalidConnectionStringException(string message, Exception inner) : base(message, inner) { }
    }
}