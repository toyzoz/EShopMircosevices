namespace Ordering.Domain.ValueObjects;

public record OrderName
{
    private const int DefaultLength = 5;

    private OrderName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static OrderName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        return new OrderName(value);
    }
}