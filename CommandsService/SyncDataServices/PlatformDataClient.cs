using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Models;
using Microsoft.Extensions.Configuration;

namespace CommandsService.SyncDataServices
{
    public class GiftSuggestionDataClient : IGiftSuggestionDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GiftSuggestionDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<GiftSuggestion> ReturnAllGiftSuggestions()
        {
            return null;
        }
    }
}