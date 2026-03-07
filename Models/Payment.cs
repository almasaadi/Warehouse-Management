namespace ManagmentSystem.Models
{
    using ManagmentSystem.Enums;

    public abstract class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        // الحالة الافتراضية
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Success;
        public DateTime PaidAt { get; set; } = DateTime.Now;
    }
}