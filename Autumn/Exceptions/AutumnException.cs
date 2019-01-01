namespace Autumn.Exceptions
{
    public class AutumnException : System.Exception
    {
        public AutumnException() : base() {}
        
        public AutumnException(string message) : base(message) {}
        
        public AutumnException(string message, System.Exception innerException) : base(message, innerException) {}
    }
}