namespace Kysect.Tamgly.Common;

public static class GenericExtensions
{
    public static T To<T>(this object source)
    {
        return (T)source;
    }
}