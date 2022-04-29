using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VoucherManagementApi.Model
{
    public class PayRequest
    {

        [StringLength(50)]
        [Display(Name = "redeemId")]
        public string RedeemId { get; set; }

        [Display(Name = "productIds")]
        public List<int> ProductIds { get; set; }
    }
}
