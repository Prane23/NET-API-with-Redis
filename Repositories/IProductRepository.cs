using NET_API_with_Redis.Model;

namespace NET_API_with_Redis.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product? Get(int id);
        Product Add(Product product);
        Product? Update(int id, Product product);
        bool Delete(int id);
    }
}
