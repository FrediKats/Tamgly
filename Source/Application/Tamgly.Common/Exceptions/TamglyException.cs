using System;

namespace Tamgly.Common.Exceptions;

public class TamglyException : Exception
{
    public TamglyException()
    {
    }

    public TamglyException(string? message)
        : base(message)
    {
    }

    public TamglyException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}