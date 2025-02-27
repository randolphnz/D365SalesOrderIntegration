﻿using Azure.Identity;
using Azure;
using SalesOrderIntegrationFunctionApp.Iservices;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrderIntegrationFunctionApp.Services
{
    internal class SecretService : ISecretService
    {
        public async Task<string> GetSecret(string secretName, string kvUrl, string keyVaultName)
        {
            SecretClient client = new SecretClient(new Uri(kvUrl), new DefaultAzureCredential());
            Response<KeyVaultSecret> secret = await client.GetSecretAsync(secretName);
            return secret.Value.Value;
        }
    }
}
