using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VoucherManagementApi.Model
{
    public class ApplyVoucherResponse
    {
        public string RedeemId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPaid { get; set; }

        public List<DiscountedProduct> Products { get; set; }
    }
    public class DiscountedProduct
    {
        public string ProductName;
        public decimal OriginalPrice;
        public decimal PriceAfterDiscount;
    }
}
