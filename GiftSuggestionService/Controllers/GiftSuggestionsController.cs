using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GiftSuggestionService.Data;
using GiftSuggestionService.Dtos;
using GiftSuggestionService.Models;
using GiftSuggestionService.Services;
using GiftSuggestionService.Utilities;
using System.Linq;
using System.Text.Json;

namespace GiftSuggestionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftSuggestionsController : ControllerBase
    {
        private readonly GptManagementService gptManagementService;
        private readonly AmazonProductManagementService amazonProductManagementService;
        private readonly RequestAccessorService requestAccessorService;
        private readonly IGiftSuggestionRepo giftSuggestionRepository;
        private readonly IAmazonProductRepo amazonProductRepository;
        private readonly IMapper mapper;

        public GiftSuggestionsController(
            GptManagementService gptManagementService,
            AmazonProductManagementService amazonProductManagementService,
            IGiftSuggestionRepo giftSuggestionRepository,
            IAmazonProductRepo amazonProductRepository,
            RequestAccessorService requestAccessorService,
            IMapper mapper)
        {
            this.gptManagementService = gptManagementService;
            this.amazonProductManagementService = amazonProductManagementService;
            this.giftSuggestionRepository = giftSuggestionRepository;
            this.amazonProductRepository = amazonProductRepository;
            this.requestAccessorService = requestAccessorService;
            this.mapper = mapper;
        }

        [HttpPost(Name = "GetGiftSuggestionsBySearchParameters")]
        public async Task<ActionResult<IEnumerable<GeneratedGiftSuggestion>>> GetGiftSuggestions(GiftSuggestionSearchDto searchParams)
        {
            return await this.gptManagementService.GetGiftSuggestions(searchParams);
        }

        [HttpPost("amazon/search", Name = "GetAmazonSuggestionsByGiftSuggestions")]
        public async Task<ActionResult<IEnumerable<AmazonProduct>>> SearchForAmazonProducts(GeneratedGiftsAndSearchParams generatedGiftsAndSearchParams)
        {
            var products = await this.GetProductsFromAmazon(generatedGiftsAndSearchParams.GeneratedGiftSuggestions, generatedGiftsAndSearchParams.SearchParams);

            return Ok(products);
        }

        [HttpPost("amazon/generate", Name = "GetAmazonSuggestionsBySearchParameters")]
        public async Task<ActionResult<IEnumerable<AmazonProduct>>> SearchAmazonSuggestionsBySearchParams(GiftSuggestionSearchDto searchParams)
        {
            // grab gift suggestions from gpt based on query paramss
            List<GeneratedGiftSuggestion> generatedGiftSuggestions = await this.gptManagementService.GetGiftSuggestions(searchParams);

            // ask amazon for 2 products
            var products = await this.GetProductsFromAmazon(generatedGiftSuggestions, searchParams);

            return Ok(products);
        }

        [HttpGet(Name = "GetAllGiftSuggestions")]
        public async Task<ActionResult<IEnumerable<GiftSuggestionReadDto>>> GetAllGiftSuggestions()
        {
            var giftSuggestionItem = await this.giftSuggestionRepository.GetAsync();

            return Ok(mapper.Map<IEnumerable<GiftSuggestionReadDto>>(giftSuggestionItem));
        }

        [HttpGet("{id}", Name = "GetGiftSuggestionById")]
        public async Task<ActionResult<GiftSuggestionReadDto>> GetGiftSuggestionById(string id)
        {
            var giftSuggestionItem = await this.giftSuggestionRepository.GetAsync(id);
            if (giftSuggestionItem != null)
            {
                return Ok(mapper.Map<GiftSuggestionReadDto>(giftSuggestionItem));
            }

            return NotFound();
        }

        [HttpPut(Name = "CreateOrUpdateGiftSuggestion")]
        public async Task<ActionResult<GiftSuggestionReadDto>> CreateOrUpdateGiftSuggestion(GiftSuggestionPutDto giftSuggestionPutDto)
        {
            var giftSuggestionModel = mapper.Map<GiftSuggestion>(giftSuggestionPutDto);

            var giftSuggestion = await this.giftSuggestionRepository.CreateOrUpdateAsync(giftSuggestionModel);

            var giftSuggestionReadDto = mapper.Map<GiftSuggestionReadDto>(giftSuggestion);

            return CreatedAtRoute(nameof(GetGiftSuggestionById), new { Id = giftSuggestionReadDto.Id }, giftSuggestionReadDto);
        }

        [HttpPost("amazon/{id}/upvotes/increment", Name = "IncrementNumOfUpvotes")]
        public async Task<ActionResult<AmazonProduct>> IncrementNumOfUpvotes(string id)
        {
            var retrievedAmazonProduct = await this.amazonProductRepository.GetAsync(id);
            if (retrievedAmazonProduct != null)
            {
                retrievedAmazonProduct.NumOfUpvotes = retrievedAmazonProduct.NumOfUpvotes + 1;
                await this.amazonProductRepository.UpdateAsync(id, retrievedAmazonProduct);
                return Ok(mapper.Map<GiftSuggestionReadDto>(retrievedAmazonProduct));
            }

            return NotFound();
        }

        [HttpPost("amazon/{id}/clicks/increment", Name = "IncrementNumOfClicks")]
        public async Task<ActionResult<AmazonProduct>> IncrementNumOfTimesClicked(string id)
        {
            var retrievedAmazonProduct = await this.amazonProductRepository.GetAsync(id);
            if (retrievedAmazonProduct != null)
            {
                retrievedAmazonProduct.NumOfClicks = retrievedAmazonProduct.NumOfClicks + 1;
                await this.amazonProductRepository.UpdateAsync(id, retrievedAmazonProduct);
                return Ok(mapper.Map<AmazonProduct>(retrievedAmazonProduct));
            }

            return NotFound();
        }

        [HttpGet("/healthcheck", Name = "HealthCheck")]
        public ActionResult HealthCheck()
        {
            return Ok();
        }

        private async Task<List<AmazonProduct>> GetProductsFromAmazon(List<GeneratedGiftSuggestion> generatedGiftSuggestions, GiftSuggestionSearchDto searchParams)
        {
            // ask amazon for 2 products
            Dictionary<string, List<AmazonProductResponseModelv2>> queryProductMapping = await this.amazonProductManagementService.GetAmazonProductDetailsFromListOfQueries(
                generatedGiftSuggestions.Select(x => x.Name).ToList(), searchParams.MaxPrice, numOfProducts: 2);

            // Create and store the product models into the DB
            List<AmazonProduct> products = new List<AmazonProduct>();
            foreach (string giftName in queryProductMapping.Keys)
            {
                List<string> ids = new List<string>();

                if (!queryProductMapping.ContainsKey(giftName))
                {
                    Console.WriteLine($"----CANT FIND KEY!!----- Searching for {giftName} but the dictionary contains: {String.Join(',', queryProductMapping.Keys)}");
                    continue;
                }

                queryProductMapping[giftName].ForEach(x =>
                {
                    bool hasRating = Double.TryParse(x.Rating, out double rating);
                    bool hasReviews = long.TryParse(x.TotalReviews, out long reviews);

                    var random = new Random();
                    AmazonProduct product = new AmazonProduct()
                    {
                        Id = $"amazon_{x.ASIN}",
                        Title = x.Title,
                        ThumbnailUrl = GiftSuggestionUtilities.ResizeThumbnailImage(x.ThumbnailUrl),
                        Link = this.amazonProductManagementService.CreateAffiliateLink(x.ASIN),
                        Price = x.Price,
                        Rating = hasRating ? rating : random.NextDouble() + 4.0,
                        NumOfReviews = hasReviews ? reviews : random.Next(70, 2000),
                    };

                    Console.WriteLine($"Suggesting product: Id={product.Id} Price={product.Price} Title={product.Title} Link={product.Link} CorrelationId={this.requestAccessorService.GetCorrelationId()}");
                    ids.Add(product.Id);
                    products.Add(this.amazonProductRepository.CreateOrUpdateAsync(product).Result);
                });

                // save gift suggestions to the db along with the products
                generatedGiftSuggestions.Where(x => x.Name == giftName).ToList().ForEach(x =>
                {
                    var giftSuggestion = x.ToPrivateModel(searchParams);

                    if (giftSuggestion.ProductIds != null)
                    {
                        giftSuggestion.ProductIds.AddRange(ids);
                    }
                    else
                    {
                        giftSuggestion.ProductIds = ids;
                    }

                    var createdGiftSuggestion = this.giftSuggestionRepository.CreateOrUpdateAsync(giftSuggestion).Result;
                });
            }

            return products;
        }
    }
}