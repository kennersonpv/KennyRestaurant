using Kenny.Services.CouponAPI.Models.Dto;

namespace Kenny.Services.CouponAPI.Repository.Interfaces
{
	public interface ICouponRepository
	{
		Task<CouponDto> GetCouponByCodeAsync(string couponCode);
	}
}
