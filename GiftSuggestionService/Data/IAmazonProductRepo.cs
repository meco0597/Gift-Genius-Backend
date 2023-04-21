namespace GiftSuggestionService.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GiftSuggestionService.Models;

    public interface IAmazonProductRepo
    {
        public Task<List<AmazonProduct>> GetAsync();

        public Task<AmazonProduct> GetAsync(string id);

        public Task<AmazonProduct> CreateOrUpdateAsync(AmazonProduct product);

        public Task CreateAsync(AmazonProduct newProduct);

        public Task UpdateAsync(string id, AmazonProduct updatedProduct);

        public Task RemoveAsync(string id);
    }
}