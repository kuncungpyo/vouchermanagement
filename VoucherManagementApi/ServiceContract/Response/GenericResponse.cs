namespace VoucherManagementApi.ServiceContract.Response
{
    public class GenericResponse<T> : BasicResponse
    {
        public T Data { get; set; }
    }
}
