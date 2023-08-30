using System;
using System.Runtime.Serialization;

namespace TodoList.Api.Exceptions
{
    [Serializable]
    public class DoesNotExistException : Exception
    {
        static readonly string ItemDoesNotExistMessage = "Item does not exist";

        public DoesNotExistException() : base(ItemDoesNotExistMessage)
        {
        }

        public DoesNotExistException(string message) : base(message)
        {
        }

        public DoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}