using System.Collections.Generic;

namespace VoucherManagementApi.Constants
{
    public static class VoucherConstants
    {
        public static readonly string DiscountTypePrice = "PRICE";
        public static readonly string DiscountTypePercentage = "PERCENTAGE";

        public static readonly List<string> DiscountTypes = new List<string>()
        {
            DiscountTypePrice,
            DiscountTypePercentage
        };
    }
}
