using System;

namespace MyCensus.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string CreditCard { get; set; }
        public int CreditCardType { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
