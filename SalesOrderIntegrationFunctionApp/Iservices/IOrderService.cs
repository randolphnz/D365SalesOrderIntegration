using Microsoft.Extensions.Logging;
using SalesOrderIntegrationFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrderIntegrationFunctionApp.Iservices
{
    public interface IOrderService
    {
        SalesOrder UpdateSalesOrder(SalesOrder order);
        Task UploadSalesOrderCSV(string sftpHost, string sftpUsername, string sftpPassword, int port,
             string filePath, string destinationPath, ILogger _logger);
    }
}
