using Kenny.Services.ShoppingCartAPI.Models.Dto;
using Kenny.Services.ShoppingCartAPI.Repository.Interfaces;

namespace Kenny.Services.ShoppingCartAPI.Repository
{
	public class CouponRepository : ICoupunRepository
	{
		public Task<CouponDto> GetCoupon(string name)
		{
			throw new NotImplementedException();
		}
	}
}
