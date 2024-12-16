namespace CurrencyRateService.Models
{
    public class UserCurrency
    {
        public int UserId { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}
