using Kenny.Services.ShoppingCartAPI.Models.Dto;

namespace Kenny.Services.ShoppingCartAPI.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<CartDto> GetCartByUserIdAsync(string userId);
        Task<CartDto> CreateUpdateCartAsync(CartDto cartDto);
        Task<bool> RemoveFromCartAsync(int cartDetailsId);
        Task<bool> ClearCartAsync(string userId);
    }
}
