namespace Kysect.Tamgly.Core;

public class TamglyException : Exception
{
    public TamglyException()
    {
    }

    public TamglyException(string? message) : base(message)
    {
    }

    public TamglyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}