using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET_API_with_Redis.Model;
using NET_API_with_Redis.Services;

namespace NET_API_with_Redis.Controllers
{
    //[Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly ProductService _productservice;

        public ProductController(ProductService productService)
        {
            _productservice = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productservice.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _productservice.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            _productservice.Add(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);  
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Product product)
        {
            var updatedProduct = _productservice.Update(id, product);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isDeleted = _productservice.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
