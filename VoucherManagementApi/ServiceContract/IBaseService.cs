using System.Net.Http;
using System.Threading.Tasks;
using VoucherManagementApi.ServiceContract.Request;
using VoucherManagementApi.ServiceContract.Response;

namespace VoucherManagementApi.ServiceContract
{
    public interface IBaseService<TDto, TDtoType>
    {
        #region Public Methods

        Task<GenericResponse<TDto>> InsertAsync(GenericRequest<TDto> request);

        Task<GenericResponse<TDto>> ReadAsync(GenericRequest<TDtoType> request);

        Task<GenericResponse<TDto>> UpdateAsync(GenericRequest<TDto> request);

        GenericResponse<TDto> Update(GenericRequest<TDto> request);

        Task<GenericResponse<TDto>> DeleteAsync(GenericRequest<TDtoType> request);

        GenericResponse<TDto> Delete(GenericRequest<TDtoType> request);

        #endregion
    }
}
