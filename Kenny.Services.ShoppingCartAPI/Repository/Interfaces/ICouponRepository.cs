using Kenny.Services.ShoppingCartAPI.Models.Dto;

namespace Kenny.Services.ShoppingCartAPI.Repository.Interfaces
{
	public interface ICoupunRepository
	{
		Task<CouponDto> GetCoupon(string name);
	}
}
