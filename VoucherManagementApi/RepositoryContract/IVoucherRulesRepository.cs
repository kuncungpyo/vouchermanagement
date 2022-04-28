using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherManagementApi.Dto;

namespace VoucherManagementApi.RepositoryContract
{
    public interface IVoucherRulesRepository : IBaseRepository<VoucherRulesDto>
    {
        Task<IEnumerable<VoucherRulesDto>> ReadByVoucherId(int voucherId);
    }
}
