namespace WhitePayment.Domain.ValueObjects
{
    public record Money(
        decimal Amount,
        string Currency = "NGN"
        );
}
