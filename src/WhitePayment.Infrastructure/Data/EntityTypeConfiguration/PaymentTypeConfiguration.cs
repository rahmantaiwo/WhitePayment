using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhitePayment.Domain;

namespace WhitePayment.Infrastructure.Data.EntityTypeConfiguration
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Reference).IsRequired();

            builder.OwnsOne(x => x.Amount, money =>
            {
                money.Property(m => m.Value).HasColumnName("Amount");
            });
        }
    }
}
