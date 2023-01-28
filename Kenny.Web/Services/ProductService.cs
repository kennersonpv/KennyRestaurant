using Kenny.Web.Models;
using Kenny.Web.Models.Dto;
using Kenny.Web.Services.IServices;

namespace Kenny.Web.Services
{
	public class ProductService : BaseService, IProductService
	{
		private const string API_PATH = "/api/products/";
		private readonly IHttpClientFactory _clientFactory;
		public ProductService(IHttpClientFactory clientFactory) : base(clientFactory)
		{
			_clientFactory = clientFactory;
		}

		public async Task<T> CreateProductAsync<T>(ProductDto productDto, string token)
		{
			return await this.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = productDto,
				Url = SD.ProductAPIBase + API_PATH,
				AccessToken = token
			});
		}

		public async Task<T> DeleteProductAsync<T>(int id, string token)
		{
			return await this.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.DELETE,
				Url = SD.ProductAPIBase + API_PATH + id,
				AccessToken = token
			});
		}

		public async Task<T> GetAllProductsAsync<T>(string token)
		{
			try
			{
				return await this.SendAsync<T>(new ApiRequest()
				{
					ApiType = SD.ApiType.GET,
					Url = SD.ProductAPIBase + API_PATH,
					AccessToken = token
				});
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public async Task<T> GetProductByIdAsync<T>(int id, string token)
		{
			return await this.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = SD.ProductAPIBase + API_PATH + id,
				AccessToken = token
			});
		}

		public async Task<T> UpdateProductAsync<T>(ProductDto productDto, string token)
		{
			return await this.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.PUT,
				Data = productDto,
				Url = SD.ProductAPIBase + API_PATH,
				AccessToken = token
			});
		}
	}
}
