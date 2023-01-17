using Kenny.Web.Models.Dto;
using Kenny.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kenny.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUserAsync());
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
			var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
			var accessToken = await HttpContext.GetTokenAsync("access_token");
			var response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);
			var cartDto = new CartDto();

			if (response != null && response.IsSuccess)
			{
                return RedirectToAction(nameof(CartIndex));
			}
            return View();
		}

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUserAsync()
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.GetCartByUserIdAsync<ResponseDto>(userId, accessToken);
            var cartDto = new CartDto();

            if(response != null && response.IsSuccess)
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }

            if(cartDto.CartHeader != null)
            {
                foreach(var detail in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += (detail.Product.Price * detail.Count);
                }
            }

            return cartDto;
        }
    }
}
