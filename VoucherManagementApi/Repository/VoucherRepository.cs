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
    public class VoucherRepository : BaseRepository<BaseDbContext, VmVoucher, VoucherDto, int>, IVoucherRepository
    {
        public VoucherRepository(
            BaseDbContext context,
            IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<VoucherDto> ReadByVoucherCode(string voucherCode)
        {
            var dbSet = this.Context.Set<VmVoucher>();

            var entity = await dbSet.FirstOrDefaultAsync(item =>
                item.Code == voucherCode);
            if (entity == null)
            {
                return null;
            }

            var dto = new VoucherDto();
            this.EntityToDto(entity, dto);
            return dto;
        }
    }
}
