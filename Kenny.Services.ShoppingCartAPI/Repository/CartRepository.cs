using AutoMapper;
using Kenny.Services.ShoppingCartAPI.DbContexts;
using Kenny.Services.ShoppingCartAPI.Models;
using Kenny.Services.ShoppingCartAPI.Models.Dto;
using Kenny.Services.ShoppingCartAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

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

            var productInDb = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);

            if (productInDb == null)
            {
                _db.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _db.SaveChangesAsync();
            }

            var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

            if(cartHeaderFromDb == null)
            {
                _db.CartHeaders.Add(cart.CartHeader);
                await _db.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _db.SaveChangesAsync();
            }
            else
            {
                var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    c => c.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                    c.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                if (cartDetailsFromDb == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }
                else
                {
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    _db.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }
            }

            var cart = _mapper.Map<Cart>(cartDto);
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
