using System.Threading.Tasks;
using VoucherManagementApi.Dto;
using VoucherManagementApi.RepositoryContract;
using VoucherManagementApi.ServiceContract;

namespace VoucherManagementApi.Service
{
    public class VoucherService : BaseService<VoucherDto, int, IVoucherRepository>, IVoucherService
    {
        public VoucherService(IVoucherRepository repository)
            : base(repository)
        {
        }

        public async Task<VoucherDto> ReadByVoucherCode(string voucherCode)
        {
            return await this._repository.ReadByVoucherCode(voucherCode);
        }

        public async Task<VoucherDto> ReadByRedeemId(string redeemId)
        {
            return await this._repository.ReadByRedeemId(redeemId);
        }
    }
}
