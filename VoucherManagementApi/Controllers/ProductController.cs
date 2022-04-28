using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VoucherManagementApi.Dto;
using VoucherManagementApi.Model;
using VoucherManagementApi.ServiceContract;
using VoucherManagementApi.ServiceContract.Request;

namespace VoucherManagementApip.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductController : BaseController
    {
        private readonly IProductService productService;

        public ProductController(
            IProductService productService,
            IConfiguration configuration)
        {
            this.productService = productService;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Pairing roster with teacher.</remarks>
        /// <response code="201">Success created.</response>
        /// <response code="401">Token is invalid.</response>
        /// <response code="403">User is not authorized to access the API.</response>
        /// <response code="500">Exception thrown.</response>
        /// 
        [HttpPost]
        [Route("/Product")]
        [Authorize("admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProductRequest model)
        {
            try
            {
                var response = await this.productService.InsertAsync(new GenericRequest<ProductDto> { 
                    Data = new ProductDto()
                    {
                        Name = model.Name,
                        Price = model.Price,
                        Color = model.Color,
                        MerchantId = model.MerchantId,
                        Stock = model.Stock,
                    }
                });

                return this.Created(new Uri("/Home/Index", UriKind.Relative), response.Data.Id);
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
        }
    }
}
