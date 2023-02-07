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

namespace GiftSuggestionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftSuggestionsController : ControllerBase
    {
        private readonly GptManagementService gptManagementService;
        private readonly IGiftSuggestionRepo repository;
        private readonly IMapper mapper;

        public GiftSuggestionsController(
            GptManagementService gptManagementService,
            IGiftSuggestionRepo repository,
            IMapper mapper)
        {
            this.gptManagementService = gptManagementService;
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet(Name = "GetAllGiftSuggestions")]
        public async Task<ActionResult<IEnumerable<GiftSuggestionReadDto>>> GetAllGiftSuggestions()
        {
            var giftSuggestionItem = await this.repository.GetAsync();

            return Ok(mapper.Map<IEnumerable<GiftSuggestionReadDto>>(giftSuggestionItem));
        }

        [HttpGet("{id}", Name = "GetGiftSuggestionById")]
        public async Task<ActionResult<GiftSuggestionReadDto>> GetGiftSuggestionById(string id)
        {
            var giftSuggestionItem = await this.repository.GetAsync(id);
            if (giftSuggestionItem != null)
            {
                return Ok(mapper.Map<GiftSuggestionReadDto>(giftSuggestionItem));
            }

            return NotFound();
        }

        [HttpPost(Name = "GetGiftSuggestionBySearchParameters")]
        public async Task<ActionResult<IEnumerable<GiftSuggestionReadDto>>> SearchGiftSuggestionsBySearchParams(GiftSuggestionSearchDto searchParams)
        {
            List<GiftSuggestion> giftSuggestions;
            int numOfGiftSuggestionsToReturn = 5;

            giftSuggestions = await this.repository.GetBySearchParameters(searchParams);
            if (giftSuggestions == null || giftSuggestions.Count < 10)
            {
                var generatedGiftSuggestions = this.gptManagementService.GetGiftSuggestions(searchParams, numOfGiftSuggestionsToReturn);
                giftSuggestions = new List<GiftSuggestion>();
                generatedGiftSuggestions.ForEach(x =>
                {
                    var giftSuggestion = x.ToPrivateModel(searchParams);
                    var createdGiftSuggestion = this.repository.CreateOrUpdateAsync(giftSuggestion).Result;
                    giftSuggestions.Add(createdGiftSuggestion);
                });
            }
            else
            {
                var random = new Random();
                for (int i = 0; i < giftSuggestions.Count - numOfGiftSuggestionsToReturn; i++)
                {
                    giftSuggestions.RemoveAt(random.Next(giftSuggestions.Count));
                }
            }

            return Ok(mapper.Map<IEnumerable<GiftSuggestionReadDto>>(giftSuggestions));
        }

        [HttpPut(Name = "CreateOrUpdateGiftSuggestion")]
        public async Task<ActionResult<GiftSuggestionReadDto>> CreateOrUpdateGiftSuggestion(GiftSuggestionPutDto giftSuggestionPutDto)
        {
            var giftSuggestionModel = mapper.Map<GiftSuggestion>(giftSuggestionPutDto);

            var giftSuggestion = await this.repository.CreateOrUpdateAsync(giftSuggestionModel);

            var giftSuggestionReadDto = mapper.Map<GiftSuggestionReadDto>(giftSuggestion);

            // TODO: Send Sync/Async Message

            return CreatedAtRoute(nameof(GetGiftSuggestionById), new { Id = giftSuggestionReadDto.Id }, giftSuggestionReadDto);
        }

        [HttpPost("{id}/upvotes/increment", Name = "IncrementNumOfUpvotes")]
        public async Task<ActionResult<GiftSuggestionReadDto>> IncrementNumOfUpvotes(string id)
        {
            var retrievedGiftSuggestion = await this.repository.GetAsync(id);
            if (retrievedGiftSuggestion != null)
            {
                retrievedGiftSuggestion.NumOfUpvotes = retrievedGiftSuggestion.NumOfUpvotes + 1;
                await this.repository.UpdateAsync(id, retrievedGiftSuggestion);
                return Ok(mapper.Map<GiftSuggestionReadDto>(retrievedGiftSuggestion));
            }

            return NotFound();
        }

        [HttpPost("{id}/clicks/increment", Name = "IncrementNumOfClicks")]
        public async Task<ActionResult<GiftSuggestionReadDto>> IncrementNumOfTimesClicked(string id)
        {
            var retrievedGiftSuggestion = await this.repository.GetAsync(id);
            if (retrievedGiftSuggestion != null)
            {
                retrievedGiftSuggestion.NumOfClicks = retrievedGiftSuggestion.NumOfClicks + 1;
                await this.repository.UpdateAsync(id, retrievedGiftSuggestion);
                return Ok(mapper.Map<GiftSuggestionReadDto>(retrievedGiftSuggestion));
            }

            return NotFound();
        }
    }
}