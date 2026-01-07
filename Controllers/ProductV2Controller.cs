using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET_API_with_Redis.Model;
using NET_API_with_Redis.Services;

namespace NET_API_with_Redis.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiVersion("2.0")] 
    [Route("api/v{version:apiVersion}/product")]   
    public class ProductV2Controller : ControllerBase
    {
        public readonly ProductService _productservice;
        private readonly RedisCacheService _cache;

        public ProductV2Controller(ProductService productService, RedisCacheService cache)
        {
            _productservice = productService;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllV2()
        {
            const string cacheKey = "products:v2";
            // 1. Try Redis first
            var cachedProducts = await _cache.GetAsync<IEnumerable<Product>>(cacheKey);

            if (cachedProducts != null) 
                return Ok(cachedProducts);

            // 2. Fetch from service
            var products = _productservice.GetAll().Where(p=>p.Id>2);

            // 3. Save to Redis for 5 minutes
            await _cache.SetAsync(cacheKey, products, TimeSpan.FromSeconds(15));

            return Ok(products);
        }
    }
}
