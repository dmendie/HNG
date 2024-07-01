namespace HNG.Abstractions.Exceptions
{
    public abstract class FoundException : Exception
    {
        protected FoundException(string message) : base(message) { }
    }
}
