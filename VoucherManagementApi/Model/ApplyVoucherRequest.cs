using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VoucherManagementApi.Model
{
    public class ApplyVoucherRequest
    {

        [StringLength(50)]
        [Display(Name = "voucherCode")]
        public string VoucherCode { get; set; }

        [Display(Name = "productIds")]
        public List<int> ProductIds { get; set; }
    }
}
