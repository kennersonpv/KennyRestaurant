using Kenny.Web.Models.Dto;
using Kenny.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kenny.Web.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<IActionResult> ProductIndex()
		{
			IEnumerable<ProductDto> productList = new List<ProductDto>();
			var response = await _productService.GetAllProductsAsync<ResponseDto>();
			if (response != null && response.IsSuccess)
			{
				productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
			}
			return View(productList);
		}

		public async Task<IActionResult> ProductCreate()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ProductCreate(ProductDto product)
		{
			if (ModelState.IsValid)
			{
				var response = await _productService.CreateProductAsync<ResponseDto>(product);
				if (response != null && response.IsSuccess)
				{
					return RedirectToAction(nameof(ProductIndex));
				}
			}
			return View(product);
		}

		public async Task<IActionResult> ProductEdit(int productId)
		{
			var response = await _productService.GetProductByIdAsync<ResponseDto>(productId);
			if (response != null && response.IsSuccess)
			{
				var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
				return View(product);
			}

			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ProductEdit(ProductDto product)
		{
			if (ModelState.IsValid)
			{
				var response = await _productService.UpdateProductAsync<ResponseDto>(product);
				if (response != null && response.IsSuccess)
				{
					return RedirectToAction(nameof(ProductIndex));
				}
			}
			return View(product);
		}

		public async Task<IActionResult> ProductDelete(int productId)
		{
			var response = await _productService.GetProductByIdAsync<ResponseDto>(productId);
			if (response != null && response.IsSuccess)
			{
				var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
				return View(product);
			}

			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ProductDelete(ProductDto product)
		{
			if (ModelState.IsValid)
			{
				var response = await _productService.DeleteProductAsync<ResponseDto>(product.ProductId);
				if (response.IsSuccess)
				{
					return RedirectToAction(nameof(ProductIndex));
				}
			}
			return View(product);
		}

	}
}
