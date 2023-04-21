namespace GiftSuggestionService.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GiftSuggestionService.Models;

    public interface IProductRepo
    {
        public Task<List<Product>> GetAsync();

        public Task<Product> GetAsync(string id);

        public Task<Product> CreateOrUpdateAsync(Product product);

        public Task CreateAsync(Product newProduct);

        public Task UpdateAsync(string id, Product updatedProduct);

        public Task RemoveAsync(string id);
    }
}