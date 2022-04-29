using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VoucherManagementApi.Constants;
using VoucherManagementApi.Dto;
using VoucherManagementApi.Model;
using VoucherManagementApi.ServiceContract;
using VoucherManagementApi.ServiceContract.Request;

namespace VoucherManagementApip.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PayController : BaseController
    {
        private readonly IProductService productService;
        private readonly IVoucherService voucherService;
        private readonly IVoucherRulesService voucherRulesService;

        public PayController(
            IProductService productService,
            IVoucherService voucherService,
            IVoucherRulesService voucherRulesService,
            IConfiguration configuration)
        {
            this.productService = productService;
            this.voucherService = voucherService;
            this.voucherRulesService = voucherRulesService;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpPost]
        [Route("/Pay")]
        [Authorize("admin")]
        public async Task<IActionResult> PayAsyncs([FromBody] PayRequest model)
        {
            try
            {
                // Validate Request
                if (model.ProductIds == null || model.ProductIds.Count == 0)
                {
                    return this.GetErrorJson("Please specify at least 1 product");
                }

                var readVoucherResponse = await this.voucherService.ReadByRedeemId(model.RedeemId);

                if (readVoucherResponse == null)
                {
                    return this.GetErrorJson("Voucher not found");
                }

                if ((DateTime.Now - readVoucherResponse.LastUsedDate).TotalSeconds > 60)
                {
                    return this.GetErrorJson("Voucher redemption has exceed the 1 minute time ");
                }

                if (readVoucherResponse.Status != "locked")
                {
                    return this.GetErrorJson("Voucher cannot be redeemed");
                }

                var selectedProducts = await this.GetSelecteProducts(model.ProductIds);

                var readVoucerRulesResponse = await this.voucherRulesService.ReadByVoucherId(readVoucherResponse.Id);

                var rulesValidatorMessage = await this.ValidateApplyVoucherRules(selectedProducts, readVoucerRulesResponse.ToList());

                if (!string.IsNullOrEmpty(rulesValidatorMessage))
                {
                    return this.GetErrorJson(rulesValidatorMessage);
                }

                // Update voucher status
                var updateVoucherRequest = readVoucherResponse;
                updateVoucherRequest.Status = "redeemed";
                updateVoucherRequest.LastUsedDate = DateTime.Now;

                await this.voucherService.UpdateAsync(new GenericRequest<VoucherDto> { Data = updateVoucherRequest });

                // Return response              

                foreach (var p in selectedProducts)
                {
                    // reduce the stock
                    p.Stock--;
                    await this.productService.UpdateAsync(new GenericRequest<ProductDto> { Data = p });
                }

                return  Ok("voucher successfuly redeemed");
            }
            catch (Exception ex)
            {
                return this.GetErrorJson("Request not valid");
            }
        }

        private async Task<List<ProductDto>> GetSelecteProducts(List<int> productIds)
        {
            List<ProductDto> selectedProducts = new List<ProductDto>();

            foreach (var productId in productIds)
            {
                var readProductResponse = await this.productService.ReadAsync(new GenericRequest<int> { Data = productId });
                if (readProductResponse != null)
                {
                    selectedProducts.Add(readProductResponse.Data);
                }
            }

            return selectedProducts;
        }

        private async Task<string> ValidateApplyVoucherRules(List<ProductDto> productDtos, List<VoucherRulesDto> voucherRules)
        {
            if (voucherRules == null || voucherRules.Count == 0)
            {
                return "";
            }

            foreach (var product in productDtos)
            {
                if (voucherRules.Any(v => v.ProductId != null))
                {
                    if (!voucherRules.Any(v => v.ProductId == product.Id))
                    {
                        return $"Cannot buy product {product.Name} using this voucher ";
                    }
                }

                if (voucherRules.Any(v => v.Color != null))
                {
                    if (!voucherRules.Any(v => v.Color == product.Color))
                    {
                        return $"Cannot buy product with color {product.Color} using this voucher ";
                    }
                }
            }

            var maxPriceRule = voucherRules.FirstOrDefault(v => v.MaximumPrice > 0);
            if (maxPriceRule != null)
            {
                if (productDtos.Sum(p => p.Price) > maxPriceRule.MaximumPrice)
                {
                    return $"Total price cannot be more than {maxPriceRule.MaximumPrice} to use this voucher";
                }
            }

            return "";
        }

        private async Task CreateVoucherRules(int voucherId, CreateVoucherRequest model)
        {
            if (model.ProductIds != null)
            {
                foreach (var productId in model.ProductIds)
                {
                    await this.voucherRulesService.InsertAsync(new GenericRequest<VoucherRulesDto>
                    {
                        Data = new VoucherRulesDto()
                        {
                            VoucherId = voucherId,
                            ProductId = productId,
                        }
                    });
                }
            }

            if (model.Colors != null)
            {
                foreach (var color in model.Colors)
                {
                    await this.voucherRulesService.InsertAsync(new GenericRequest<VoucherRulesDto>
                    {
                        Data = new VoucherRulesDto()
                        {
                            VoucherId = voucherId,
                            Color = color,
                        }
                    });
                }
            }

            if (model.MaximumPrice > 0)
            {
                await this.voucherRulesService.InsertAsync(new GenericRequest<VoucherRulesDto>
                {
                    Data = new VoucherRulesDto()
                    {
                        VoucherId = voucherId,
                        MaximumPrice = model.MaximumPrice,
                    }
                });
            }
        }
    }
}
