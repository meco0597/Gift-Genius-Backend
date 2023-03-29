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
        private readonly IProductRepo productRepository;
        private readonly IMapper mapper;

        public GiftSuggestionsController(
            GptManagementService gptManagementService,
            AmazonProductManagementService amazonProductManagementService,
            IGiftSuggestionRepo giftSuggestionRepository,
            IProductRepo productRepository,
            RequestAccessorService requestAccessorService,
            IMapper mapper)
        {
            this.gptManagementService = gptManagementService;
            this.amazonProductManagementService = amazonProductManagementService;
            this.giftSuggestionRepository = giftSuggestionRepository;
            this.productRepository = productRepository;
            this.requestAccessorService = requestAccessorService;
            this.mapper = mapper;
        }

        [HttpPost(Name = "GetGiftSuggestionBySearchParameters")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchGiftSuggestionsBySearchParams(GiftSuggestionSearchDto searchParams)
        {
            List<GiftSuggestion> giftSuggestions;
            List<GeneratedGiftSuggestion> generatedGiftSuggestions;
            int numOfGiftSuggestionsToReturn = 5;

            // check the db for existing entries for the search parameters
            giftSuggestions = await this.giftSuggestionRepository.GetBySearchParameters(searchParams);

            // if less than 10 entries available, go to gpt to generate some more
            if (giftSuggestions == null || giftSuggestions.Count < 10)
            {
                generatedGiftSuggestions = await this.gptManagementService.GetGiftSuggestions(searchParams);
            }
            else
            {
                // else grab numOfGiftSuggestionsToReturn random suggestions
                var random = new Random();
                for (int i = 0; i < giftSuggestions.Count - numOfGiftSuggestionsToReturn; i++)
                {
                    giftSuggestions.RemoveAt(random.Next(giftSuggestions.Count));
                }

                generatedGiftSuggestions = new List<GeneratedGiftSuggestion>();
                giftSuggestions.ForEach(x =>
                {
                    generatedGiftSuggestions.Add(new GeneratedGiftSuggestion()
                    {
                        Categories = x.AssociatedInterests,
                        Name = x.GiftName,
                    });
                });
            }

            // ask amazon for 3 (3 is default for now) products with the lowest relavancy score
            Dictionary<string, Task<List<AmazonProductResponseModelv2>>> queryProductMapping = this.amazonProductManagementService.GetAmazonProductDetailsFromListOfQueries(
                generatedGiftSuggestions.Select(x => x.Name).ToList(), searchParams.MaxPrice, numOfProducts: 2).Result;

            // Create and store the product models into the DB
            List<Product> products = new List<Product>();
            foreach (string giftName in queryProductMapping.Keys)
            {
                List<string> ids = new List<string>();
                queryProductMapping[giftName].Result.ForEach(x =>
                {
                    Product product = new Product()
                    {
                        Id = $"amazon_{x.ASIN}",
                        Title = x.Title,
                        ThumbnailUrl = x.ThumbnailUrl,
                        Link = this.amazonProductManagementService.CreateAffiliateLink(x.ASIN),
                        Price = x.Price,
                    };

                    Console.WriteLine($"Suggesting product: Id={product.Id} Price={product.Price} Title={product.Title} Link={product.Link} CorrelationId={this.requestAccessorService.GetCorrelationId()}");
                    ids.Add(product.Id);
                    products.Add(this.productRepository.CreateOrUpdateAsync(product).Result);
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

        [HttpPost("{id}/upvotes/increment", Name = "IncrementNumOfUpvotes")]
        public async Task<ActionResult<GiftSuggestionReadDto>> IncrementNumOfUpvotes(string id)
        {
            var retrievedGiftSuggestion = await this.giftSuggestionRepository.GetAsync(id);
            if (retrievedGiftSuggestion != null)
            {
                retrievedGiftSuggestion.NumOfUpvotes = retrievedGiftSuggestion.NumOfUpvotes + 1;
                await this.giftSuggestionRepository.UpdateAsync(id, retrievedGiftSuggestion);
                return Ok(mapper.Map<GiftSuggestionReadDto>(retrievedGiftSuggestion));
            }

            return NotFound();
        }

        [HttpPost("{id}/clicks/increment", Name = "IncrementNumOfClicks")]
        public async Task<ActionResult<GiftSuggestionReadDto>> IncrementNumOfTimesClicked(string id)
        {
            var retrievedGiftSuggestion = await this.giftSuggestionRepository.GetAsync(id);
            if (retrievedGiftSuggestion != null)
            {
                retrievedGiftSuggestion.NumOfClicks = retrievedGiftSuggestion.NumOfClicks + 1;
                await this.giftSuggestionRepository.UpdateAsync(id, retrievedGiftSuggestion);
                return Ok(mapper.Map<GiftSuggestionReadDto>(retrievedGiftSuggestion));
            }

            return NotFound();
        }

        [HttpGet("/healthcheck", Name = "HealthCheck")]
        public ActionResult HealthCheck()
        {
            return Ok();
        }
    }
}