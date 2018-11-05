using System;

namespace CustomerManagement.Database
{
    public class OpenDatabaseException : Exception
    {
        public OpenDatabaseException() { }

        public OpenDatabaseException(string message) : base(message) { }

        public OpenDatabaseException(string message, Exception inner) : base(message, inner) { }
    }
}