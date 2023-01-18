using Kenny.MessageBus;
using Kenny.Services.ShoppingCartAPI.Messages;
using Kenny.Services.ShoppingCartAPI.Models.Dto;
using Kenny.Services.ShoppingCartAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kenny.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartAPIController : Controller
    {
        private readonly ICartRepository _cartRepository;
		private readonly ICouponRepository _couponRepository;
		private readonly IMessageBus _messageBus;
		protected ResponseDto _response;

        public CartAPIController(ICartRepository cartRepository, IMessageBus messageBus, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository;
            _messageBus = messageBus;
            _couponRepository = couponRepository;
            this._response = new ResponseDto();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserIdAsync(userId);
                _response.Result = cartDto;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDto cartDto)
        {
            try
            {
                var cart = await _cartRepository.CreateUpdateCartAsync(cartDto);
                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart(CartDto cartDto)
        {
            try
            {
                var cart = await _cartRepository.CreateUpdateCartAsync(cartDto);
                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                bool isRemoved = await _cartRepository.RemoveFromCartAsync(cartDetailsId);
                _response.Result = isRemoved;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

		[HttpPost("ApplyCoupon")]
		public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
		{
			try
			{
				bool isRemoved = await _cartRepository.ApplyCouponAsync(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
				_response.Result = isRemoved;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpPost("RemoveCoupon")]
		public async Task<object> RemoveCoupon([FromBody] string userId)
		{
			try
			{
				bool isRemoved = await _cartRepository.RemoveCouponAsync(userId);
				_response.Result = isRemoved;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpPost("Checkout")]
		public async Task<object> Checkout(CheckoutHeaderDto checkoutHeader)
		{
			try
			{
                var cartDto = await _cartRepository.GetCartByUserIdAsync(checkoutHeader.UserId);
                if(cartDto == null)
                {
                    return BadRequest();
                }

                if (!string.IsNullOrEmpty(checkoutHeader.CouponCode))
                {
                    var coupon = await _couponRepository.GetCoupon(checkoutHeader.CouponCode);
                    if(checkoutHeader.DiscountTotal != coupon.DiscountAmount)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string>() { "Coupon Price has changed, please confirm" };
                        _response.DisplayMessage = "Coupon Price has changed, please confirm";
                        return _response;
					}
                }

                checkoutHeader.CartDetails = cartDto.CartDetails;
                await _messageBus.PublicMessage(checkoutHeader, "checkoutmessagetopic");

			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;
		}
	}
}
