namespace NoBeard.Emails.DeliveryCheck.Models;

public class AcsEmailEventData
{
    public string Sender { get; set; }

    public string Recipient { get; set; }

    public string InternetMessageId { get; set; }

    public string MessageId { get; set; }

    public string Status { get; set; }

    public DeliveryStatusDetails DeliveryStatusDetails { get; set; }

    public DateTime DeliveryAttemptTimestamp { get; set; }
}