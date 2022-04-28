namespace VoucherManagementApi.Dto
{
    public class ProductDto : BaseDto<int>
    {
        public string Name{ get; set; }
        public int MerchantId { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public int Stock { get; set; }
    }
}
