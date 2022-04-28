using System.Threading.Tasks;

namespace VoucherManagementApi.RepositoryContract
{
    public interface IBaseRepository<TDto>
    {
        Task<TDto> InsertAsync(TDto dto);

        Task<TDto> ReadAsync(object primaryKey);

        Task<TDto> UpdateAsync(TDto dto);

        TDto Update(TDto dto);

        Task<TDto> DeleteAsync(object primaryKey);

        TDto Delete(object primaryKey);
    }
}
