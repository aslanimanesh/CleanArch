﻿namespace MyApp.Domain.ViewModels.Discounts
{
    public class DiscountViewModel
    {
        public int Id { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DiscountCode { get; set; }
        public bool IsActive { get; set; }

    }
}
