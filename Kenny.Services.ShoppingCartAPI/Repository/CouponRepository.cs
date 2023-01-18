using Kenny.Services.ShoppingCartAPI.Models.Dto;
using Kenny.Services.ShoppingCartAPI.Repository.Interfaces;
using Newtonsoft.Json;

namespace Kenny.Services.ShoppingCartAPI.Repository
{
	public class CouponRepository : ICoupunRepository
	{
		private readonly HttpClient client;
		private const string API_PATH = "/api/coupon/";

		public CouponRepository(HttpClient client)
		{
			this.client = client;
		}

		public async Task<CouponDto> GetCoupon(string couponName)
		{
			var httpResponse = await client.GetAsync(API_PATH + couponName);
			var apiContent = await httpResponse.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
			if (response != null && response.IsSuccess)
			{
				return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
			}
			return new CouponDto();
		}
	}
}
