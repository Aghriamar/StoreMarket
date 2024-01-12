using Microsoft.AspNetCore.Mvc;
using StoreMarket.Models;

namespace StoreMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet("getProducts")]
        public ActionResult GetProducts()
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var products = context.Products.Select(x => new Product()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    });
                    return Ok(products);
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost("putProducts")]
        public ActionResult PutProducts([FromQuery] string name, string description, int categoryId, int price)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if(!context.Products.Any(x => x.Name.ToLower().Equals(name)))
                    {
                        context.Add(new Product()
                        {
                            Name = name,
                            Description = description,
                            Price = price,
                            CategoryId = categoryId
                        });
                        context.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(409);
                    }
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
