using NET_API_with_Redis.Model;
using NET_API_with_Redis.Repositories;

namespace NET_API_with_Redis.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product? Get(int id)
        {
            return _productRepository.Get(id);
        }

        public Product Add(Product product)
        {
            return _productRepository.Add(product);
        }

        public Product Update(int id, Product product)
        {
            return _productRepository.Update(id, product);
        }
        public bool Delete(int id)
        {
            return _productRepository.Delete(id);
        }

    }
}
