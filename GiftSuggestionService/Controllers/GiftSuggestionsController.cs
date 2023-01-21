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

namespace GiftSuggestionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftSuggestionsController : ControllerBase
    {
        private readonly IGiftSuggestionRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public GiftSuggestionsController(
            IGiftSuggestionRepo repository,
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GiftSuggestionReadDto>> GetGiftSuggestions()
        {
            Console.WriteLine("--> Getting GiftSuggestions....");

            var giftSuggestionItem = _repository.GetAsync();

            return Ok(_mapper.Map<IEnumerable<GiftSuggestionReadDto>>(giftSuggestionItem));
        }

        [HttpGet("{id}", Name = "GetGiftSuggestionById")]
        public ActionResult<GiftSuggestionReadDto> GetGiftSuggestionById(Guid id)
        {
            var giftSuggestionItem = _repository.GetAsync(id);
            if (giftSuggestionItem != null)
            {
                return Ok(_mapper.Map<GiftSuggestionReadDto>(giftSuggestionItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<GiftSuggestionReadDto>> CreateGiftSuggestion(GiftSuggestionPutDto giftSuggestionPutDto)
        {
            var giftSuggestionModel = _mapper.Map<GiftSuggestion>(giftSuggestionPutDto);
            await _repository.CreateAsync(giftSuggestionModel);

            var giftSuggestionReadDto = _mapper.Map<GiftSuggestionReadDto>(giftSuggestionModel);

            // Send Sync Message
            try
            {
                await _commandDataClient.SendGiftSuggestionToCommand(giftSuggestionReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            //Send Async Message TODO: not supported yet
            // try
            // {
            //     var giftSuggestionPublishedDto = _mapper.Map<GiftSuggestionPublishedDto>(giftSuggestionReadDto);
            //     giftSuggestionPublishedDto.Event = "GiftSuggestion_Published";
            //     _messageBusClient.PublishNewGiftSuggestion(giftSuggestionPublishedDto);
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            // }

            return CreatedAtRoute(nameof(GetGiftSuggestionById), new { Id = giftSuggestionReadDto.Id }, giftSuggestionReadDto);
        }
    }
}