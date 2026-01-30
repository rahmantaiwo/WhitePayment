namespace WhitePayment.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
