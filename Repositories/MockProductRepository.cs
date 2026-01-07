using NET_API_with_Redis.Model;
using NET_API_with_Redis.Repositories;

namespace NET_API_with_Redis.Service
{
    public class MockProductRepository : IProductRepository
    {
        public readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Price = 1000.00m },
            new Product { Id = 2, Name = "Kayboard", Price = 20.0m },
            new Product { Id = 3, Name = "Mouse", Price = 10.0m }
        };

        public Product Add(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
            return product;
        }

        public bool Delete(int id)
        {
            var product = Get(id);
            if (product == null)
            {
                return false;
            }
            _products.Remove(product);
            return true;

        }

        public Product? Get(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product? Update(int id, Product product)
        {
            var existingproduct = Get(id);
            if (existingproduct == null)
            {
                return null;
            }

            existingproduct.Name = product.Name;
            existingproduct.Price = product.Price;
            return existingproduct;


        }
    }
}
