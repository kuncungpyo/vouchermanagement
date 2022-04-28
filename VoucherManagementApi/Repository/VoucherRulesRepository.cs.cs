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
    public class VoucherRulesRepository : BaseRepository<BaseDbContext, VmVoucherRules, VoucherRulesDto, int>, IVoucherRulesRepository
    {
        public VoucherRulesRepository(
            BaseDbContext context,
            IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<IEnumerable<VoucherRulesDto>> ReadByVoucherId(int voucherId)
        {
            var dbSet = this.Context.Set<VmVoucherRules>();
            var entityList = await dbSet.Where(item => item.VoucherId == voucherId).ToListAsync();

            var dtoList = new List<VoucherRulesDto>();

            foreach (var entity in entityList)
            {
                var dto = new VoucherRulesDto();
                this.EntityToDto(entity, dto);
                dtoList.Add(dto);
            }

            return dtoList;
        }
    }
}
