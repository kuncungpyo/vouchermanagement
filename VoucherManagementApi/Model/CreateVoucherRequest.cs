using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VoucherManagementApi.Model
{
    public class CreateVoucherRequest
    {

        [StringLength(50)]
        [Display(Name = "code")]
        public string Code { get; set; }

        [Display(Name = "discount")]
        public decimal Discount { get; set; }

        [StringLength(10)]
        [Display(Name = "discountType")]
        public string DiscountType { get; set; }

        [StringLength(10)]
        [Display(Name = "status")]
        public string Status { get; set; }

        [Display(Name = "expiredDate")]
        public DateTime ExpiredDate { get; set; }

        [Display(Name = "lastUsedDate")]
        public DateTime LastUsedDate { get; set; }

        [Display(Name = "productIds")]
        public List<int> ProductIds { get; set; }

        [Display(Name = "colors")]
        public List<string> Colors { get; set; }

        [Display(Name = "maximumPrice")]
        public decimal MaximumPrice { get; set; }
    }
}
