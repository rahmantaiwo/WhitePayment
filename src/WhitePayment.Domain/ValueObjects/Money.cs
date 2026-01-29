namespace WhitePayment.Domain.ValueObjects
{public sealed class Money
    {
        public decimal Value { get; }

        public Money(decimal value)
        {
            if (value <= 0)
                throw new ArgumentException("Amount must be greater than zero");

            Value = value;
        }
    }
}



//www.indeed.com / r / ABDUL - QUADIR - RAHMAN - TAIWO / bc6135302ae305e8
//www.indeed.com/r/ABDUL-QUADIR-RAHMAN-TAIWO/bc6135302ae305e8
