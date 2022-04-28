using System.ComponentModel.DataAnnotations;

namespace VoucherManagementApi.Model
{
    public class CreateProductRequest
    {
        public int MerchantId { get; set; }

        [StringLength(50)]
        [Display(Name = "name")]
        public string Name { get; set; }

        [Display(Name = "price")]
        public decimal Price { get; set; }

        [StringLength(10)]
        [Display(Name = "color")]
        public string Color { get; set; }

        [Display(Name = "stock")]
        public int Stock { get; set; }
    }
}
