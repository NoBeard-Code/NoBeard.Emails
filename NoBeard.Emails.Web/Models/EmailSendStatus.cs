using System.Text.Json.Serialization;

namespace NoBeard.Emails.Web.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmailSendStatus
{
    FailedToSend = 0,
    Sent = 1,
    Skipped = 2,
    Pending = 3,

    // Azure statuses
    Delivered = 10,
    Suppressed = 11,
    Bounced = 12,
    Quarantined = 13,
    FilteredSpam = 14,
    Expanded = 15,
    Failed = 16
}