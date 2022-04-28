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
    public class VoucherController : BaseController
    {
        private readonly IProductService productService;
        private readonly IVoucherService voucherService;
        private readonly IVoucherRulesService voucherRulesService;

        public VoucherController(
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
        [Route("/Voucher")]
        [Authorize("admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateVoucherRequest model)
        {
            try
            {
                if (!VoucherConstants.DiscountTypes.Contains(model.DiscountType))
                {
                    return this.GetErrorJson("Discount type must be either PRICE or PERCENTAGE");
                }

                var response = await this.voucherService.InsertAsync(new GenericRequest<VoucherDto>
                {
                    Data = new VoucherDto()
                    {
                        Code = model.Code,
                        Discount = model.Discount,
                        DiscountType = model.DiscountType,
                        ExpiredDate = model.ExpiredDate,
                        Status = "active"
                    }
                });

                if (response.Data == null)
                {
                    return this.GetErrorJson("Failed to create voucher");
                }

                await this.CreateVoucherRules(response.Data.Id, model);

                return this.Created(new Uri("/Home/Index", UriKind.Relative), response.Data.Id);
            }
            catch (Exception ex)
            {
                return this.GetErrorJson(ex.Message);
            }
        }

        [HttpPost]
        [Route("/Voucher/Apply")]
        [Authorize("admin")]
        public async Task<IActionResult> ApplyVoucherAsync([FromBody] ApplyVoucherRequest model)
        {
            try
            {
                // Validate Request
                if (model.ProductIds == null || model.ProductIds.Count == 0)
                {
                    return this.GetErrorJson("Please specify at least 1 product");
                }

                var readVoucherResponse = await this.voucherService.ReadByVoucherCode(model.VoucherCode);

                if (readVoucherResponse == null)
                {
                    return this.GetErrorJson("Voucher not found");
                }

                if (readVoucherResponse.ExpiredDate <= DateTime.Now)
                {
                    return this.GetErrorJson("Voucher expired");
                }

                if (readVoucherResponse.Status != "active")
                {
                    return this.GetErrorJson("Voucher cannot be used");
                }

                var selectedProducts = await this.GetSelecteProducts(model.ProductIds);
                if (selectedProducts.Any(p => p.Stock == 0))
                {
                    return this.GetErrorJson("One of the product is out of stock");
                }

                var readVoucerRulesResponse = await this.voucherRulesService.ReadByVoucherId(readVoucherResponse.Id);

                var rulesValidatorMessage = await this.ValidateApplyVoucherRules(selectedProducts, readVoucerRulesResponse.ToList());

                if (!string.IsNullOrEmpty(rulesValidatorMessage))
                {
                    return this.GetErrorJson(rulesValidatorMessage);
                }


                // Update voucher status
                var updateVoucherRequest = readVoucherResponse;
                updateVoucherRequest.Status = "locked";
                updateVoucherRequest.LastUsedDate = DateTime.Now;

                await this.voucherService.UpdateAsync(new GenericRequest<VoucherDto> { Data = updateVoucherRequest });

                // Return response              

                var response = new ApplyVoucherResponse()
                {
                    RedeemId = Guid.NewGuid(),
                    TotalPrice = selectedProducts.Sum(p => p.Price),
                    TotalDiscount = readVoucherResponse.DiscountType == VoucherConstants.DiscountTypePercentage ?
                        selectedProducts.Sum(p => p.Price * readVoucherResponse.Discount / 100)
                        : selectedProducts.Count * readVoucherResponse.Discount,
                    Products = new List<DiscountedProduct>()
                };

                foreach (var p in selectedProducts)
                {
                    var discountPrice = readVoucherResponse.DiscountType == VoucherConstants.DiscountTypePercentage ?
                        p.Price * readVoucherResponse.Discount / 100
                        : readVoucherResponse.Discount;

                    var discountedProduct = new DiscountedProduct
                    {
                        ProductName = p.Name,
                        OriginalPrice = p.Price,
                        PriceAfterDiscount = p.Price - discountPrice,
                    };

                    response.Products.Add(discountedProduct);
                }

                response.TotalPaid = response.TotalPrice - response.TotalDiscount;

                return  Ok(response);
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
