namespace GiftSuggestionService.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GiftSuggestionService.Models;
    using GiftSuggestionService.Configurations;
    using GiftSuggestionService.Services;
    using GiftSuggestionService.Utilities;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using GiftSuggestionService.Dtos;
    using System.Linq.Expressions;

    public class ProductRepo : IProductRepo
    {
        private readonly IMongoCollection<Product> productCollection;
        private readonly Dbconfiguration dbConfiguration;

        public ProductRepo(
            IOptionsMonitor<Dbconfiguration> optionsMonitor,
            KeyvaultAccessorService keyvaultAccessor)
        {
            this.dbConfiguration = optionsMonitor.CurrentValue;
            var connectionString = keyvaultAccessor.GetSecretAsync(this.dbConfiguration.KeyvaultSecretName).Result;

            var mongoClient = new MongoClient(connectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                this.dbConfiguration.DatabaseName);

            this.productCollection = mongoDatabase.GetCollection<Product>(
                this.dbConfiguration.ProductCollectionName);
        }

        public async Task<Product> CreateOrUpdateAsync(Product product)
        {
            string id = product.Id;
            var exisitingProduct = await this.GetAsync(product.Id);
            if (exisitingProduct == null)
            {
                // create
                Product createdProduct = new Product()
                {
                    Id = id,
                    Title = product.Title,
                    Link = product.Link,
                    ThumbnailUrl = product.ThumbnailUrl,
                    Price = product.Price,
                    NumOfUpvotes = 0,
                    NumOfClicks = 0,
                    NumOfTimesSuggested = 1,
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now,
                };

                await this.CreateAsync(createdProduct);
                return createdProduct;
            }
            else
            {
                // update 
                Product updatedProduct = new Product()
                {
                    Id = id,
                    Title = product.Title ?? exisitingProduct.Title,
                    Price = product.Price == default(double) ? exisitingProduct.Price : product.Price,
                    Link = product.Link ?? exisitingProduct.Link,
                    ThumbnailUrl = product.Link ?? exisitingProduct.ThumbnailUrl,
                    NumOfUpvotes = exisitingProduct.NumOfUpvotes,
                    NumOfClicks = exisitingProduct.NumOfClicks,
                    NumOfTimesSuggested = exisitingProduct.NumOfTimesSuggested + 1,
                    CreatedAt = exisitingProduct.CreatedAt,
                    LastModifiedAt = DateTime.Now,
                };

                await this.UpdateAsync(id, updatedProduct);
                return updatedProduct;
            }
        }

        public async Task<List<Product>> GetAsync() =>
            await this.productCollection.Find(_ => true).ToListAsync();

        public async Task<Product> GetAsync(string id) =>
            await this.productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product newProduct) =>
            await this.productCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, Product updatedProduct) =>
            await this.productCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await this.productCollection.DeleteOneAsync(x => x.Id == id);
    }
}