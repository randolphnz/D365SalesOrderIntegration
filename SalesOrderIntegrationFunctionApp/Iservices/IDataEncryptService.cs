using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrderIntegrationFunctionApp.Iservices
{
    public interface IDataEncryptService
    {
        void EncryptCSVFile(string inputFilePath, string outputFilePath, string password);
    }
}
