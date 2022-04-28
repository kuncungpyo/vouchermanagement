using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherManagementApi.Dto;

namespace VoucherManagementApi.ServiceContract
{
    public interface IVoucherRulesService : IBaseService<VoucherRulesDto, int>
    {
        Task<IEnumerable<VoucherRulesDto>> ReadByVoucherId(int voucherId);
    }
}
