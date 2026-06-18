namespace BTVN.Models
{
    public class AccountModel
    {
        public int STT { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
