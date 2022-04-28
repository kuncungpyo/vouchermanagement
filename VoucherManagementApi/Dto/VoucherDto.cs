using System;

namespace VoucherManagementApi.Dto
{
    public class VoucherDto : BaseDto<int>
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public string DiscountType { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime LastUsedDate { get; set; }
        public string Status { get; set; }
    }
}
