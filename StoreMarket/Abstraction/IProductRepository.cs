using Microsoft.Extensions.Caching.Memory;
using StoreMarket.Models;
using StoreMarket.Models.DTO;

namespace StoreMarket.Abstraction
{
    public interface IProductRepository
    {
        public int AddCategory(CategoryDTO category);
        public IEnumerable<CategoryDTO> GetCategories();
        public int AddProduct(ProductDTO product);
        public IEnumerable<ProductDTO> GetProducts();
        public MemoryCacheStatistics? GetCacheStatistics();
    }
}
