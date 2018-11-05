using System;

namespace CustomerManagement.Database
{
    public class MismatchingUpdateArgumentException : Exception
    {
        public MismatchingUpdateArgumentException() { }

        public MismatchingUpdateArgumentException(string message) : base(message) { }

        public MismatchingUpdateArgumentException(string message, Exception inner) : base(message, inner) { }
    }
}