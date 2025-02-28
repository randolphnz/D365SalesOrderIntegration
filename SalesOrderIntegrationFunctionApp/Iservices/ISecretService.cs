using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrderIntegrationFunctionApp.Iservices
{
    public interface ISecretService
    {
        Task<string> GetSecret(SecretClient client, string secretName);
    }
}
