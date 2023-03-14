using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using GiftSuggestionService.Models;
using GiftSuggestionService.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GiftSuggestionService.Services
{
    public class KeyvaultAccessorService
    {
        private readonly KeyvaultConfiguration keyvaultConfiguration;
        private readonly SecretClient secretClient;

        public KeyvaultAccessorService(
            IOptionsMonitor<KeyvaultConfiguration> optionsMonitor)
        {
            this.keyvaultConfiguration = optionsMonitor.CurrentValue;
            this.secretClient = new SecretClient(new Uri(
                this.keyvaultConfiguration.keyvaultUri),
                new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions
                    {
                        ExcludeSharedTokenCacheCredential = true
                    }));
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var secret = await this.secretClient.GetSecretAsync(secretName);
            return secret.Value.Value;
        }
    }
}