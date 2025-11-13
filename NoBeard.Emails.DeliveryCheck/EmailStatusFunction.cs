using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NoBeard.Emails.DeliveryCheck.Models;
using System.Text.Json;

namespace NoBeard.Emails.DeliveryCheck;

public class EmailStatusFunction(ILogger<EmailStatusFunction> logger)
{
    private readonly ILogger<EmailStatusFunction> _logger = logger;

    [Function(nameof(EmailStatusFunction))]
    public async Task Run([QueueTrigger("emailevents", Connection = "StorageAccount")] QueueMessage message)
    {
        _logger.LogInformation($"Received queue message: {message.MessageText}");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Deserialize ACS event payload
        var acsEvent = JsonSerializer.Deserialize<AcsEmailEvent>(message.Body.ToString(), options);

        // Example: check status
        if (acsEvent?.Data?.Status == "Delivered" || acsEvent?.Data?.Status == "Expanded")
        {
            // Update your domain DB record
            await UpdateEmailRecordAsync(acsEvent.Id, acsEvent?.Data?.Status);
            _logger.LogInformation($"Email {acsEvent.Id} marked as {acsEvent?.Data?.Status}");
        }
        else
        {
            // Trigger business process
            await HandleFailureAsync(acsEvent);
        }
    }

    private Task UpdateEmailRecordAsync(string emailId, string? status)
    {
        // TODO: implement DB update
        return Task.CompletedTask;
    }

    private Task HandleFailureAsync(AcsEmailEvent acsEvent)
    {
        // TODO: implement business logic
        return Task.CompletedTask;
    }
}