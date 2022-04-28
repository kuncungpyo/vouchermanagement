using System;

namespace VoucherManagementApi.Dto
{
    public class VoucherRulesDto : BaseDto<int>
    {
        public int VoucherId { get; set; }
        public int? ProductId { get; set; }
        public string Color { get; set; }
        public decimal MaximumPrice { get; set; }
    }
}
