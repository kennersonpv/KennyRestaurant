using AutoMapper;
using Kenny.Services.ShoppingCartAPI.DbContexts;
using Kenny.Services.ShoppingCartAPI.Models;
using Kenny.Services.ShoppingCartAPI.Models.Dto;
using Kenny.Services.ShoppingCartAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Kenny.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public CartRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<CartDto> CreateUpdateCartAsync(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);

            var productInDb = _db.Products.FirstOrDefault(p => p.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);

            if (productInDb == null)
            {
                _db.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _db.SaveChangesAsync();
            }


            throw new NotImplementedException();
        }

        public async Task<CartDto> GetCartByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveFromCartAsync(int cartDetailsId)
        {
            throw new NotImplementedException();
        }
    }
}
