using AutoMapper;
using VoucherManagementApi.DataAccess.Entities;
using VoucherManagementApi.Dto;

namespace VoucherManagementApi.DataAccess.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<VmVoucher, VoucherDto>().ReverseMap();
            this.CreateMap<VmProduct, ProductDto>().ReverseMap();
            this.CreateMap<VmVoucherRules, VoucherRulesDto>().ReverseMap();
        }
    }
}
