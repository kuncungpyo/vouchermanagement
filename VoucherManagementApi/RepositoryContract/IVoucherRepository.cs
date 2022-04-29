using System.Threading.Tasks;
using VoucherManagementApi.Dto;

namespace VoucherManagementApi.RepositoryContract
{
    public interface IVoucherRepository : IBaseRepository<VoucherDto>
    {
        Task<VoucherDto> ReadByVoucherCode(string voucherCode);

        Task<VoucherDto> ReadByRedeemId(string redeemId);
    }
}
