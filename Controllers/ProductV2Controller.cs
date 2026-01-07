using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ProductV2Controller(ProductService productService)
        {
            _productservice = productService;
        }

        [HttpGet]
        public IActionResult GetAllV2()
        {
            var products = _productservice.GetAll().Where(p=>p.Id>2);
            return Ok(products);
        }
    }
}
