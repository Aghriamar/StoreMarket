using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using StoreMarket.Abstraction;
using StoreMarket.Models;
using StoreMarket.Models.DTO;

namespace StoreMarket.Repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public ProductRepository(IMapper mapper, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public int AddCategory(CategoryDTO category)
        {
            using (var context = new ProductContext())
            {
                var entityGroup = context.Categories.FirstOrDefault(x => x.Name.ToLower() == category.Name.ToLower());
                if (entityGroup == null)
                {
                    entityGroup = _mapper.Map<Category>(category);
                    context.Categories.Add(entityGroup);
                    context.SaveChanges();
                    _memoryCache.Remove("categories");
                }
                return entityGroup.Id;
            }
        }

        public int AddProduct(ProductDTO product)
        {
            using (var context = new ProductContext())
            {
                var entityProduct = context.Products.FirstOrDefault(x => x.Name.ToLower() == product.Name.ToLower());
                if (entityProduct == null)
                {
                    entityProduct = _mapper.Map<Product>(product);
                    context.Products.Add(entityProduct);
                    context.SaveChanges();
                    _memoryCache.Remove("products");
                }
                return entityProduct.Id;
            }
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            if (_memoryCache.TryGetValue("categories", out List<CategoryDTO> categories))
            {
                return categories;
            }
            using (var context = new ProductContext())
            {
                var categoriesList = context.Categories.Select(x => _mapper.Map<CategoryDTO>(x)).ToList();
                _memoryCache.Set("categories", categoriesList, TimeSpan.FromMinutes(30));
                return categoriesList;
            }
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            if (_memoryCache.TryGetValue("products", out List<ProductDTO> products))
            {
                return products;
            }
            using (var context = new ProductContext())
            {
                var productsList = context.Products.Select(x => _mapper.Map<ProductDTO>(x)).ToList();
                _memoryCache.Set("products", productsList, TimeSpan.FromMinutes(30));
                return productsList;
            }
        }

        public MemoryCacheStatistics? GetCacheStatistics()
        {
            return _memoryCache.GetCurrentStatistics();
        }
    }
}
