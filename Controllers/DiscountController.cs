using System.Net;
using System.Threading.Tasks;
using Discount.Api.Entities;
using Discount.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        #region constractor
        private readonly IDiscountRepository _discountRepository;
        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
        #endregion

        #region get discount
        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(type: typeof(Coupon),statusCode:(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
           var result = await _discountRepository.GetDiscount(productName: productName);
           return Ok(result);
        }
        #endregion

        #region create discount
        [HttpPost]
        [ProducesResponseType(type: typeof(Coupon), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            var result = await _discountRepository.CreateDiscount(coupon);
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName });
        }
        #endregion

        #region update discount
        [HttpPut]
        [ProducesResponseType(type: typeof(bool), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> UpdateDiscount([FromBody] Coupon coupon)
        {
            return Ok(await _discountRepository.UpdateDiscount(coupon));
        }
        #endregion

        #region delete discount

        [HttpDelete("{productName}")]
        [ProducesResponseType(type: typeof(bool), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        {
            return Ok(await _discountRepository.DeleteDiscount(productName: productName));
        }

        #endregion

    }
}
