using System.Threading.Tasks;
using VoucherManagementApi.Dto;

namespace VoucherManagementApi.ServiceContract
{
    public interface IVoucherService : IBaseService<VoucherDto, int>
    {
        Task<VoucherDto> ReadByVoucherCode(string voucherCode);

        Task<VoucherDto> ReadByRedeemId(string redeemId);
    }
}
