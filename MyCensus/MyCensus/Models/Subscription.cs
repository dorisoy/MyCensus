﻿using System;

namespace MyCensus.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string CreditCard { get; set; }
        public string CreditCardType { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
