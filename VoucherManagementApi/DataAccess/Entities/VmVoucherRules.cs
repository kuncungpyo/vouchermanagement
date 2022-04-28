using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoucherManagementApi.DataAccess.Entities
{
    [Table("vm_voucher_rules")]
    public class VmVoucherRules
    {
        [Key]
        public int Id { get; set; }

        public int VoucherId { get; set; }

        public int? ProductId { get; set; }

        [StringLength(50)]
        public string Color { get; set; }

        public decimal MaximumPrice { get; set; }
    }
}
