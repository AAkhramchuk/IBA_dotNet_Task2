using System;

namespace WpfApp2.Exceptions
{
    [Serializable]
    public class ApplicationException : Exception
    {
        private static readonly string DefaultMessage = "Неожиданная ошибка.";

        public ApplicationException() : base(DefaultMessage) { }
        public ApplicationException(string message) : base(message) { }
        public ApplicationException(string message, Exception innerException)
        : base(message, innerException) { }

        protected ApplicationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    }
}
