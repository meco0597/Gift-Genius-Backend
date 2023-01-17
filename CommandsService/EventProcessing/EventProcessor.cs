using System;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.GiftSuggestionPublished:
                    addGiftSuggestion(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notifcationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

            switch (eventType.Event)
            {
                case "GiftSuggestion_Published":
                    Console.WriteLine("--> GiftSuggestion Published Event Detected");
                    return EventType.GiftSuggestionPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void addGiftSuggestion(string giftSuggestionPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var giftSuggestionPublishedDto = JsonSerializer.Deserialize<GiftSuggestionPublishedDto>(giftSuggestionPublishedMessage);

                try
                {
                    var plat = _mapper.Map<GiftSuggestion>(giftSuggestionPublishedDto);
                    if (!repo.ExternalGiftSuggestionExists(plat.ExternalID))
                    {
                        repo.CreateGiftSuggestion(plat);
                        repo.SaveChanges();
                        Console.WriteLine("--> GiftSuggestion added!");
                    }
                    else
                    {
                        Console.WriteLine("--> GiftSuggestion already exisits...");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add GiftSuggestion to DB {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        GiftSuggestionPublished,
        Undetermined
    }
}