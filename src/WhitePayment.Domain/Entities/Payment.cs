using WhitePayment.Domain.Common;
using WhitePayment.Domain.Enum;
using WhitePayment.Domain.ValueObjects;

namespace WhitePayment.Domain
{
    public class Payment : AuditableEntity
    {
        public string Reference { get; private set; }
        public string Email { get; private set; }
        public Money Amount { get; private set; }
        public PaymentStatus Status { get; private set; }

        private Payment() { }

        public Payment(
            string reference, 
            string email, 
            Money amount)
        {
            Reference = reference;
            Email = email;
            Amount = amount;
            Status = PaymentStatus.Pending;
        }

        public void MarkSuccessful()
        {
            Status = PaymentStatus.Successful;
        }

        public void MarkFailed()
        {
            Status = PaymentStatus.Failed;
        }
    }
}
