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

    public class AmazonProductRepo : IAmazonProductRepo
    {
        private readonly IMongoCollection<AmazonProduct> productCollection;
        private readonly Dbconfiguration dbConfiguration;

        public AmazonProductRepo(
            IOptionsMonitor<Dbconfiguration> optionsMonitor,
            KeyvaultAccessorService keyvaultAccessor)
        {
            this.dbConfiguration = optionsMonitor.CurrentValue;
            var connectionString = keyvaultAccessor.GetSecretAsync(this.dbConfiguration.KeyvaultSecretName).Result;

            var mongoClient = new MongoClient(connectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                this.dbConfiguration.DatabaseName);

            this.productCollection = mongoDatabase.GetCollection<AmazonProduct>(
                this.dbConfiguration.ProductCollectionName);
        }

        public async Task<AmazonProduct> CreateOrUpdateAsync(AmazonProduct product)
        {
            string id = product.Id;
            var exisitingProduct = await this.GetAsync(product.Id);
            if (exisitingProduct == null)
            {
                // create
                AmazonProduct createdProduct = new AmazonProduct()
                {
                    Id = id,
                    Title = product.Title,
                    Link = product.Link,
                    ThumbnailUrl = product.ThumbnailUrl,
                    Price = product.Price,
                    NumOfReviews = product.NumOfReviews,
                    Rating = product.Rating,
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
                product.Price = exisitingProduct.Price + 1;
                // update 
                AmazonProduct updatedProduct = new AmazonProduct()
                {
                    Id = id,
                    Title = product.Title ?? exisitingProduct.Title,
                    Price = product.Price == default(double) ? exisitingProduct.Price : product.Price,
                    Rating = product.Rating == default(double) ? exisitingProduct.Rating : product.Rating,
                    NumOfReviews = product.NumOfReviews == default(long) ? exisitingProduct.NumOfReviews : product.NumOfReviews,
                    Link = product.Link ?? exisitingProduct.Link,
                    ThumbnailUrl = product.ThumbnailUrl ?? exisitingProduct.ThumbnailUrl,
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

        public async Task<List<AmazonProduct>> GetAsync() =>
            await this.productCollection.Find(_ => true).ToListAsync();

        public async Task<AmazonProduct> GetAsync(string id) =>
            await this.productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(AmazonProduct newProduct) =>
            await this.productCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, AmazonProduct updatedProduct) =>
            await this.productCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await this.productCollection.DeleteOneAsync(x => x.Id == id);
    }
}