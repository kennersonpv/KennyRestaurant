using Kenny.Web.Models.Dto;

namespace Kenny.Web.Services.IServices
{
    public interface ICartService
    {
        Task<T> GetCartByUserIdAsync<T>(string userId, string token = null);
        Task<T> AddToCartByUserIdAsync<T>(CartDto cartDto, string token = null);
        Task<T> UdateCartByUserIdAsync<T>(CartDto cartDto, string token = null);
        Task<T> RemoveFromCartAsync<T>(int cartId, string token = null);
    }
}
