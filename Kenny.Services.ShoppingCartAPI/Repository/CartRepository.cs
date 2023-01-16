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
            var cartHeaderFromDb = await _db.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
            if(cartHeaderFromDb != null)
            {
                _db.CartDetails.RemoveRange(_db.CartDetails.Where(u => u.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                _db.CartHeaders.Remove(cartHeaderFromDb);
                await _db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
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

            return _mapper.Map<CartDto>(cartDto);
        }

        public async Task<CartDto> GetCartByUserIdAsync(string userId)
        {
            var cart = new Cart()
            {
                CartHeader = await _db.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId)
            };

            cart.CartDetails = _db.CartDetails.Where(c => c.CartHeaderId == cart.CartHeader.CartHeaderId).Include(c => c.Product);

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveFromCartAsync(int cartDetailsId)
        {
            throw new NotImplementedException();
        }
    }
}
