using Kenny.Web.Models;
using Kenny.Web.Services.IServices;

namespace Kenny.Web.Services
{
	public class CouponService: BaseService, ICouponService
	{
		private const string API_PATH = "/api/coupon/";
		private readonly IHttpClientFactory _httpClient;

		public CouponService(IHttpClientFactory httpClient) : base(httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<T> GetCouponAsync<T>(string couponCode, string token = null)
		{
			return await this.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = SD.CouponAPIBase + API_PATH + couponCode,
				AccessToken = token
			});
		}
	}
}
