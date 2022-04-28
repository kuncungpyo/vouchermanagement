using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoucherManagementApi.DataAccess.Entities
{
    [Table("vm_voucher")]
    public class VmVoucher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        [StringLength(10)]
        public string DiscountType { get; set; }

        [Required]
        public DateTime ExpiredDate { get; set; }

        [Required]
        public DateTime LastUsedDate { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }
    }
}
