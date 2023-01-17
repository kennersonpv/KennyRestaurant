using Kenny.Web.Models;
using Kenny.Web.Models.Dto;
using Kenny.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Kenny.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var productList = new List<ProductDto>();
            var response = await _productService.GetAllProductsAsync<ResponseDto>("");
            if(response != null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            return View(productList);
        }

        [Authorize]
        [HttpPost]
        [ActionName("Details")]
		public async Task<IActionResult> DetailsPost(ProductDto productDto)
		{
            var cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId
            };

            var response = await _productService.GetProductByIdAsync<ResponseDto>(productDto.ProductId, "");
            if(response != null && response.IsSuccess)
            {
                cartDetails.Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }

            var cartDetailsDtos = new List<CartDetailsDto>();
            cartDetailsDtos.Add(cartDetails);
            cartDto.CartDetails = cartDetailsDtos;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var addToCartResp = await _cartService.AddToCartAsync<ResponseDto>(cartDto, accessToken);
            if (addToCartResp != null && addToCartResp.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDto);
		}

        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            var productDtoModel = new ProductDto();
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, "");
            if (response != null && response.IsSuccess)
            {
                productDtoModel = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            return View(productDtoModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
		public async Task<IActionResult> Login()
		{
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Logout()
		{
			return SignOut("Cookies","oidc");
		}
	}
}