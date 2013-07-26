namespace nJinja.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EnvironmentException : Exception
    {
        public EnvironmentException() { }

        public EnvironmentException(string message) : base(message) { }

        public EnvironmentException(string message, Exception innerException) : base(message, innerException) { }

        protected EnvironmentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}