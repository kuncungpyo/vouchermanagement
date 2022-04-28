using VoucherManagementApi.Dto;
using VoucherManagementApi.RepositoryContract;
using VoucherManagementApi.ServiceContract;

namespace VoucherManagementApi.Service
{
    public class ProductService : BaseService<ProductDto, int, IProductRepository>, IProductService
    {
        public ProductService(IProductRepository repository)
            : base(repository)
        {
        }
    }
}
