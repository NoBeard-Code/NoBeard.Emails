namespace NoBeard.Emails.DeliveryCheck.Models;

public class AcsEmailEvent
{
    public string Id { get; set; }

    public string Topic { get; set; }

    public string Subject { get; set; }

    public AcsEmailEventData Data { get; set; }

    public string EventType { get; set; }

    public string DataVersion { get; set; }

    public string MetadataVersion { get; set; }

    public DateTime EventTime { get; set; }
}


