using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoucherManagementApi.DataAccess.Entities
{
    [Table("vm_product")]
    public class VmProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MerchantId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name{ get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(10)]
        public string Color { get; set; }

        [Required]
        public int Stock { get; set; }
    }
}
