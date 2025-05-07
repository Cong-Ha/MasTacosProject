namespace MasTacos.DTO
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool MarketingOptIn { get; set; }
        public DateTime JoinDate { get; set; }
        public int LoyaltyPoints { get; set; }
    }
}
