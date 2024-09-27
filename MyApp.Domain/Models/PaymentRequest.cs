namespace MyApp.Domain.Models
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }

    }

}
