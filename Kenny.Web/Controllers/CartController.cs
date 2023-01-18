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
		private readonly ICouponService _couponService;

		public CartController(IProductService productService, ICartService cartService, ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUserAsync());
        }

        [HttpPost]
        [ActionName("ApplyCoupon")]
		public async Task<IActionResult> ApplyCoupon(CartDto cart)
		{
			var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
			var accessToken = await HttpContext.GetTokenAsync("access_token");
			var response = await _cartService.ApplyCouponAsync<ResponseDto>(cart, accessToken);

			if (response != null && response.IsSuccess)
			{
				return RedirectToAction(nameof(CartIndex));
			}
            return View();
		}

		[HttpPost]
		[ActionName("RemoveCoupon")]
		public async Task<IActionResult> RemoveCoupon(CartDto cart)
		{
			var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
			var accessToken = await HttpContext.GetTokenAsync("access_token");
			var response = await _cartService.RemoveCouponAsync<ResponseDto>(cart.CartHeader.UserId, accessToken);

			if (response != null && response.IsSuccess)
			{
				return RedirectToAction(nameof(CartIndex));
			}
			return View();
		}

		public async Task<IActionResult> Remove(int cartDetailsId)
        {
			var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
			var accessToken = await HttpContext.GetTokenAsync("access_token");
			var response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);

			if (response != null && response.IsSuccess)
			{
                return RedirectToAction(nameof(CartIndex));
			}
            return View();
		}

		
		public async Task<IActionResult> Checkout()
		{
			return View(await LoadCartDtoBasedOnLoggedInUserAsync());
		}

		[HttpPost]
		public async Task<IActionResult> Checkout(CartDto cartDto)
		{
			try
			{
				var accessToken = await HttpContext.GetTokenAsync("access_token");
				var response = await _cartService.CheckoutAsync<ResponseDto>(cartDto.CartHeader, accessToken);
				if(response != null && !response.IsSuccess) 
				{
					ViewBag.Error = response.DisplayMessage;
					return RedirectToAction(nameof(Checkout));
				}

				return RedirectToAction(nameof(Confirmation));
			}
			catch(Exception )
			{
				return View(cartDto);
			}
		}

		
		public async Task<IActionResult> Confirmation()
		{
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
                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    var couponResponse = await _couponService.GetCouponAsync<ResponseDto>(cartDto.CartHeader.CouponCode, accessToken);
					if (couponResponse != null && couponResponse.IsSuccess)
					{
						var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(couponResponse.Result));
                        cartDto.CartHeader.DiscountTotal = coupon.DiscountAmount;
					}
				}

                foreach(var detail in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += (detail.Product.Price * detail.Count);
                }

                cartDto.CartHeader.OrderTotal -= cartDto.CartHeader.DiscountTotal;
            }

            return cartDto;
        }
    }
}
