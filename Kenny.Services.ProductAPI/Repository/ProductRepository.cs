﻿using AutoMapper;
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

        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            var product = _mapper.Map<ProductDto, Product>(productDto);
            if(product.ProductId > 0)
            {
                _db.Products.Update(product);
            }
            else
            {
                _db.Products.Add(product);
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<Product, ProductDto>(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
                if(product== null)
                {
                    return false;
                }
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return true;
            }catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            var product = await _db.Products.Where(p => p.ProductId == productId).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            IEnumerable<Product> productList = await _db.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(productList);
        }
    }
}
