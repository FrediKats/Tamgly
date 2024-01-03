namespace Tamgly.Mapping;

public interface IMapper<TFirst, TSecond>
{
    TSecond Map(TFirst value);
    TFirst Map(TSecond value);
}