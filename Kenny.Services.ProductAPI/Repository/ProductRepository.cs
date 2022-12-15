using AutoMapper;
using Kenny.Services.ProductAPI.DbContexts;
using Kenny.Services.ProductAPI.Models;
using Kenny.Services.ProductAPI.Models.Dto;
using Kenny.Services.ProductAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kenny.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;   
        }

        public Task<ProductDto> CreateUpdateProduct(ProductDto product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            var product = await _db.Products.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            IEnumerable<Product> productList = await _db.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(productList);
        }
    }
}
