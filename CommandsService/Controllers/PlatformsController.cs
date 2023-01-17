using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class GiftSuggestionsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public GiftSuggestionsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GiftSuggestionreadDto>> GetGiftSuggestions()
        {
            Console.WriteLine("--> Getting GiftSuggestions from CommandsService");

            var giftSuggestionItems = _repository.GetAllGiftSuggestions();

            return Ok(_mapper.Map<IEnumerable<GiftSuggestionreadDto>>(giftSuggestionItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test of from GiftSuggestions Controler");
        }
    }
}