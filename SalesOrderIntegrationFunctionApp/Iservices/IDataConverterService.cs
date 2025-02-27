using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrderIntegrationFunctionApp.Iservices
{
    public interface IDataConverterService
    {
        string JsonToCSV(string jsonData);
    }
}
