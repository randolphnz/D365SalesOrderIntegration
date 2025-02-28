using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SalesOrderIntegrationFunctionApp.Iservices;
using SalesOrderIntegrationFunctionApp.Models;

namespace SalesOrderIntegrationFunctionApp
{
    public class D365IntegrationFunction
    {

        private readonly ILogger _logger;
        private readonly ISecretService _secretService;
        private readonly SecretClient _secretClient;
        private readonly IDataEncryptService _dataEncryptService;
        private readonly IDataConverterService _dataConverterService;
        private readonly IOrderService _orderService;

        public D365IntegrationFunction(ILogger<D365IntegrationFunction> logger, SecretClient secretClient, ISecretService secretService, 
            IDataEncryptService dataEncryptServices, IDataConverterService dataConverterServices, IOrderService orderService)
        {
            _logger = logger;
            _secretClient = secretClient;
            _secretService = secretService;
            _dataEncryptService = dataEncryptServices;
            _dataConverterService = dataConverterServices;
            _orderService = orderService;
        }

        [Function(nameof(D365IntegrationFunction))]
        public async Task Run([ServiceBusTrigger("myqueue", Connection = "")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation($"Function executed at: {DateTime.Now}");
            try
            {
                if (message == null)
                {
                    _logger.LogError($"Received message is null at: {DateTime.Now}");
                    return;
                }

                // Deserialize json to .net object
                SalesOrder salesOrder = JsonSerializer.Deserialize<SalesOrder>(message.ToString());
                if(salesOrder == null)
                {
                    _logger.LogError($"json deserialized failed at: {DateTime.Now}");
                    return;
                }

                _logger.LogInformation($"Ready for dispatch event json deserialized at: {DateTime.Now}");
                // Container conversion
                salesOrder = _orderService.UpdateSalesOrder(salesOrder);
                _logger.LogInformation($"Container conversion at: {DateTime.Now}");

                // Serialize object to json
                string jsonString = JsonSerializer.Serialize(salesOrder);
                string csv = _dataConverterService.JsonToCSV(jsonString);
                File.WriteAllText("output.csv", csv);

                string sftpHost = await _secretService.GetSecret(_secretClient, "secretName");
                _logger.LogInformation($"Sftp host fetched at: {DateTime.Now}");
                string sftpUsername = await _secretService.GetSecret(_secretClient, "secretName");
                _logger.LogInformation($"Sftp Username fetched at: {DateTime.Now}");
                string sftpPassword = await _secretService.GetSecret(_secretClient, "secretName");
                _logger.LogInformation($"Sftp Password fetched at: {DateTime.Now}");
                string port = await _secretService.GetSecret(_secretClient, "secretName");
                _logger.LogInformation($"Sftp Port fetched at: {DateTime.Now}");
                string password = await _secretService.GetSecret(_secretClient, "secretName");
                _logger.LogInformation($"Password fetched at: {DateTime.Now}");

                // Encrypt CSV
                _dataEncryptService.EncryptCSVFile("output.csv", "output_encrypted.csv", password);
                _logger.LogInformation($"CSV encrypted at: {DateTime.Now}");

                //Upload to SFTP
                await _orderService.UploadSalesOrderCSV(sftpHost, sftpUsername, sftpPassword, int.Parse(port), "output_encrypted.csv", "/bluecorp-incoming", _logger);
                _logger.LogInformation($"Encrypted csv uploaded at: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
            }
        }
    }
}
