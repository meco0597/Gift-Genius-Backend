using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/giftSuggestions/{giftSuggestionId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForGiftSuggestion(int giftSuggestionId)
        {
            Console.WriteLine($"--> Hit GetCommandsForGiftSuggestion: {giftSuggestionId}");

            if (!_repository.PlaformExits(giftSuggestionId))
            {
                return NotFound();
            }

            var commands = _repository.GetCommandsForGiftSuggestion(giftSuggestionId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForGiftSuggestion")]
        public ActionResult<CommandReadDto> GetCommandForGiftSuggestion(int giftSuggestionId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForGiftSuggestion: {giftSuggestionId} / {commandId}");

            if (!_repository.PlaformExits(giftSuggestionId))
            {
                return NotFound();
            }

            var command = _repository.GetCommand(giftSuggestionId, commandId);

            if (command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForGiftSuggestion(int giftSuggestionId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForGiftSuggestion: {giftSuggestionId}");

            if (!_repository.PlaformExits(giftSuggestionId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);

            _repository.CreateCommand(giftSuggestionId, command);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForGiftSuggestion),
                new { giftSuggestionId = giftSuggestionId, commandId = commandReadDto.Id }, commandReadDto);
        }

    }
}