using Kenny.Services.ShoppingCartAPI.Models.Dto;

namespace Kenny.Services.ShoppingCartAPI.Repository.Interfaces
{
	public interface ICouponRepository
	{
		Task<CouponDto> GetCoupon(string name);
	}
}
