using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VoucherManagementApi.DataAccess.Application;
using VoucherManagementApi.DataAccess.Entities;
using VoucherManagementApi.Dto;
using VoucherManagementApi.Repository;
using VoucherManagementApi.RepositoryContract;

namespace VoucherManagementApi.Repository
{
    public class ProductRepository : BaseRepository<BaseDbContext, VmProduct, ProductDto, int>, IProductRepository
    {
        public ProductRepository(
            BaseDbContext context,
            IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
