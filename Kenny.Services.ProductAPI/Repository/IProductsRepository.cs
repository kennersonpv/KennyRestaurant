using Kenny.Services.ProductAPI.Models.Dto;

namespace Kenny.Services.ProductAPI.Repository
{
    public interface IProductsRepository
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int productId);
        Task<ProductDto> CreateUpdateProduct(ProductDto product);
        Task<bool> DeleteProduct(int productId);
    }
}
