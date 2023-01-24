using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GiftSuggestionService.AsyncDataServices;
using GiftSuggestionService.Data;
using GiftSuggestionService.Dtos;
using GiftSuggestionService.Models;
using GiftSuggestionService.SyncDataServices.Http;
using GiftSuggestionService.Utilities;

namespace GiftSuggestionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftSuggestionsController : ControllerBase
    {
        private readonly IGiftSuggestionRepo _repository;
        private readonly IMapper _mapper;

        public GiftSuggestionsController(
            IGiftSuggestionRepo repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GiftSuggestionReadDto>>> GetGiftSuggestions()
        {
            Console.WriteLine("--> Getting GiftSuggestions....");

            var giftSuggestionItem = await _repository.GetAsync();

            return Ok(_mapper.Map<IEnumerable<GiftSuggestionReadDto>>(giftSuggestionItem));
        }

        [HttpGet("{id}", Name = "GetGiftSuggestionById")]
        public async Task<ActionResult<GiftSuggestionReadDto>> GetGiftSuggestionById(string id)
        {
            var giftSuggestionItem = await _repository.GetAsync(id);
            if (giftSuggestionItem != null)
            {
                return Ok(_mapper.Map<GiftSuggestionReadDto>(giftSuggestionItem));
            }

            return NotFound();
        }

        [HttpPost("{id}", Name = "PostIncrementNumOfUpvotes")]
        public async Task<ActionResult<GiftSuggestionReadDto>> IncrementNumOfUpvotes(string id)
        {
            var retrievedGiftSuggestion = await _repository.GetAsync(id);
            if (retrievedGiftSuggestion != null)
            {
                retrievedGiftSuggestion.NumOfUpvotes = retrievedGiftSuggestion.NumOfUpvotes + 1;
                await _repository.UpdateAsync(id, retrievedGiftSuggestion);
                return Ok(_mapper.Map<GiftSuggestionReadDto>(retrievedGiftSuggestion));
            }

            return NotFound();
        }

        [HttpPut]
        public async Task<ActionResult<GiftSuggestionReadDto>> CreateGiftSuggestion(GiftSuggestionPutDto giftSuggestionPutDto)
        {
            var giftSuggestionModel = _mapper.Map<GiftSuggestion>(giftSuggestionPutDto);

            string id = GiftSuggestionUtilities.GenerateGiftSuggestionIdFromName(giftSuggestionPutDto.GiftName);
            var retrievedGiftSuggestion = await _repository.GetAsync(id);

            GiftSuggestionReadDto giftSuggestionReadDto;
            // Create
            if (retrievedGiftSuggestion == null)
            {
                await _repository.CreateAsync(giftSuggestionModel);
                giftSuggestionReadDto = _mapper.Map<GiftSuggestionReadDto>(giftSuggestionModel);
            }
            // Update
            else
            {
                giftSuggestionModel.NumOfUpvotes = retrievedGiftSuggestion.NumOfUpvotes;
                await _repository.UpdateAsync(id, giftSuggestionModel);
                giftSuggestionReadDto = _mapper.Map<GiftSuggestionReadDto>(giftSuggestionModel);
            }

            // TODO: Send Sync/Async Message

            return CreatedAtRoute(nameof(GetGiftSuggestionById), new { Id = giftSuggestionReadDto.Id }, giftSuggestionReadDto);
        }
    }
}