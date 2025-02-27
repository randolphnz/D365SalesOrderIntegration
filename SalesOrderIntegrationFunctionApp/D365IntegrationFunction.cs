using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SalesOrderIntegrationFunctionApp
{
    public class D365IntegrationFunction
    {
        private readonly ILogger<D365IntegrationFunction> _logger;

        public D365IntegrationFunction(ILogger<D365IntegrationFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(D365IntegrationFunction))]
        public async Task Run([ServiceBusTrigger("myqueue", Connection = "")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
