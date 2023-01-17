using Kenny.Services.CouponAPI.Models.Dto;
using Kenny.Services.CouponAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kenny.Services.CouponAPI.Controllers
{
	[ApiController]
	[Route("api/coupon")]
	public class CouponController : Controller
	{
		private readonly ICouponRepository _couponRepository;
		protected ResponseDto _response;

		public CouponController(ICouponRepository couponRepository)
		{
			_couponRepository = couponRepository;
			_response = new ResponseDto();
		}

		[HttpGet("{couponCode}")]
		public async Task<object> GetCoupon(string couponCode)
		{
			try
			{
				var couponDto = await _couponRepository.GetCouponByCodeAsync(couponCode);
				_response.Result = couponDto;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Result = new List<string>() { ex.ToString() };
			}
			return _response;
		}
	}
}
