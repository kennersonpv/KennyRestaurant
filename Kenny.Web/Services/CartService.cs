using Kenny.Web.Models;
using Kenny.Web.Models.Dto;
using Kenny.Web.Services.IServices;

namespace Kenny.Web.Services
{
    public class CartService : BaseService, ICartService
    {
        private const string API_PATH = "/api/cart/";
        private readonly IHttpClientFactory _httpClient;

        public CartService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> AddToCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + API_PATH + "AddCart",
                AccessToken = token
            });
        }

		public async Task<T> ApplyCouponAsync<T>(CartDto cartDto, string token = null)
		{
			return await this.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = cartDto,
				Url = SD.ShoppingCartAPIBase + API_PATH + "ApplyCoupon",
				AccessToken = token
			});
		}

		public async Task<T> CheckoutAsync<T>(CartHeaderDto cartHeaderDto, string token = null)
		{
			return await this.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = cartHeaderDto,
				Url = SD.ShoppingCartAPIBase + API_PATH + "Checkout",
				AccessToken = token
			});
		}

		public async Task<T> GetCartByUserIdAsync<T>(string userId, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartAPIBase + API_PATH + "GetCart/" + userId,
                AccessToken = token
            });
        }

		public async Task<T> RemoveCouponAsync<T>(string userId, string token = null)
		{
			return await this.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = userId,
				Url = SD.ShoppingCartAPIBase + API_PATH + "RemoveCoupon",
				AccessToken = token
			});
		}

		public async Task<T> RemoveFromCartAsync<T>(int cartId, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartId,
                Url = SD.ShoppingCartAPIBase + API_PATH + "RemoveCart",
                AccessToken = token
            });
        }

        public async Task<T> UdateCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + API_PATH + "UpdateCart",
                AccessToken = token
            });
        }
    }
}
