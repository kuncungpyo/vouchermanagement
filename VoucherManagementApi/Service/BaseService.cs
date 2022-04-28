using System.Threading.Tasks;
using VoucherManagementApi.Dto;
using VoucherManagementApi.RepositoryContract;
using VoucherManagementApi.ServiceContract;
using VoucherManagementApi.ServiceContract.Request;
using VoucherManagementApi.ServiceContract.Response;

namespace VoucherManagementApi.Service
{
    public abstract class BaseService<TDto, TDtoType, TRepository> : IBaseService<TDto, TDtoType>
        where TDto : BaseDto<TDtoType>
        where TRepository : IBaseRepository<TDto>
    {
        protected readonly TRepository _repository;

        protected BaseService(TRepository repository)
        {
            _repository = repository;
        }

        public virtual async Task<GenericResponse<TDto>> InsertAsync(GenericRequest<TDto> request)
        {
            var response = new GenericResponse<TDto>();

            if (response.IsError()) return response;

            var result = await _repository.InsertAsync(request.Data);

            response.Data = result;
            response.AddInfoMessage("Saved");

            return response;
        }

        public virtual async Task<GenericResponse<TDto>> ReadAsync(GenericRequest<TDtoType> request)
        {
            var response = new GenericResponse<TDto>();

            var dto = await _repository.ReadAsync(request.Data);
            if (dto == null)
            {
                response.AddErrorMessage("Not Found");
                return response;
            }

            response.Data = dto;

            return response;
        }

        public virtual async Task<GenericResponse<TDto>> UpdateAsync(GenericRequest<TDto> request)
        {
            var response = new GenericResponse<TDto>();

            if (response.IsError()) return response;

            var dto = await _repository.UpdateAsync(request.Data);
            if (dto == null)
            {
                response.AddErrorMessage("Not Found");
                return response;
            }

            response.Data = dto;
            response.AddInfoMessage("Saved");

            return response;
        }
        
        public virtual GenericResponse<TDto> Update(GenericRequest<TDto> request)
        {
            var response = new GenericResponse<TDto>();

            if (response.IsError()) return response;

            var dto = _repository.Update(request.Data);
            if (dto == null)
            {
                response.AddErrorMessage("Not Found");
                return response;
            }

            response.Data = dto;
            response.AddInfoMessage("Saved");

            return response;
        }

        public virtual async Task<GenericResponse<TDto>> DeleteAsync(GenericRequest<TDtoType> request)
        {
            var response = new GenericResponse<TDto>();

            var dto = await _repository.DeleteAsync(request.Data);
            if (dto == null)
            {
                response.AddErrorMessage("Not Found");
                return response;
            }

            response.Data = dto;
            response.AddInfoMessage("Deleted");

            return response;
        }

        public virtual GenericResponse<TDto> Delete(GenericRequest<TDtoType> request)
        {
            var response = new GenericResponse<TDto>();

            var dto = _repository.Delete(request.Data);
            if (dto == null)
            {
                response.AddErrorMessage("Not Found");
                return response;
            }

            response.Data = dto;
            response.AddInfoMessage("Deleted");

            return response;
        }
    }
}
