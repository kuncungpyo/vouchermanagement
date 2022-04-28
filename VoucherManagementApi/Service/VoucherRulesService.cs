using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherManagementApi.Dto;
using VoucherManagementApi.RepositoryContract;
using VoucherManagementApi.ServiceContract;

namespace VoucherManagementApi.Service
{
    public class VoucherRulesService : BaseService<VoucherRulesDto, int, IVoucherRulesRepository>, IVoucherRulesService
    {
        public VoucherRulesService(IVoucherRulesRepository repository)
            : base(repository)
        {
        }

        public async Task<IEnumerable<VoucherRulesDto>> ReadByVoucherId(int voucherId)
        {
            var dtos = await this._repository.ReadByVoucherId(voucherId);

            return dtos;
        }
    }
}
