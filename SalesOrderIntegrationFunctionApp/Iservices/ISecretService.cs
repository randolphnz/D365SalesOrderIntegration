using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrderIntegrationFunctionApp.Iservices
{
    public interface ISecretService
    {
        Task<string> GetSecret(string secretName, string kvUrl, string keyVaultName);
    }
}
