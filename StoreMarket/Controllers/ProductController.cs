using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreMarket.Abstraction;
using StoreMarket.Models;
using StoreMarket.Models.DTO;
using System.Globalization;
using System.Text;

namespace StoreMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("getProducts")]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("getCategories")]
        public IActionResult GetCategories()
        {
            var categories = _productRepository.GetCategories();
            return Ok(categories);
        }

        [HttpPost("putProducts")]
        public IActionResult PutProducts([FromBody] ProductDTO productDTO)
        {
            var result = _productRepository.AddProduct(productDTO);
            return Ok(result);
        }

        [HttpPost("addCategory")]
        public IActionResult AddCategory([FromBody] CategoryDTO categoryDTO)
        {
            var result = _productRepository.AddCategory(categoryDTO);
            return Ok(result);
        }

        [HttpGet("exportProductsCsv")]
        public IActionResult ExportProductsCsv()
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var products = context.Products.Select(x => new Product()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    }).ToList();
                    var memoryStream = new MemoryStream();
                    var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
                    var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
                    csvWriter.WriteRecords(products);
                    streamWriter.Flush();
                    memoryStream.Position = 0;
                    return File(memoryStream, "text/csv", "product.csv");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("cacheStatistics")]
        public IActionResult CacheStatistics()
        {
            try
            {
                var statistics = _productRepository.GetCacheStatistics();

                if (statistics != null)
                {
                    var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
                    Directory.CreateDirectory(staticFilesPath);

                    var filePath = Path.Combine(staticFilesPath, "CacheStatistics.txt");
                    System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(statistics));

                    return PhysicalFile(filePath, "text/plain", "CacheStatistics.txt");
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpDelete("deleteCategory")]
        public ActionResult DeleteProducts(int categoryId)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var categoryToDelete = context.Categories.Find(categoryId);
                    if (categoryToDelete != null)
                    {
                        context.Categories.Remove(categoryToDelete);
                        context.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("deleteProduct")]
        public ActionResult DeleteProduct(int productId)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var productToDelete = context.Products.Find(productId);
                    if (productToDelete != null)
                    {
                        context.Products.Remove(productToDelete);
                        context.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("setProductPrice")]
        public ActionResult SetProductPrice(int productId, [FromQuery] int newPrice)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var productToUpdate = context.Products.Find(productId);
                    if (productToUpdate != null)
                    {
                        productToUpdate.Price = newPrice;
                        context.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return NoContent();
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
