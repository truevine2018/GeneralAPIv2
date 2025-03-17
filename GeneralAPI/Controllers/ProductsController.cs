using GeneralAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _shopContext;

        public ProductsController(ShopContext shopcontext)
        {
            _shopContext = shopcontext;

            _shopContext.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            return Ok(await _shopContext.Products.ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            if (product == null)
            { 
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAvailableProduct()
        {
            return await _shopContext.Products.Where(p => p.IsAvailable == false).ToArrayAsync();
        }
        [HttpPost]
        public async Task<ActionResult> PostProduct(Product product)
        {
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = product.Id },
                product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _shopContext.Entry(product).State = EntityState.Modified;

            try
            {
                await _shopContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_shopContext.Products.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            _shopContext.Products.Remove(product);
            await _shopContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsCategory(int id)
        {
            var productCategory = await _shopContext.Products.Where(p => p.CategoryId == id).ToArrayAsync();
            if (productCategory is null)
            {
                return NotFound();
            }

            return Ok(productCategory);
        }
    }
}
