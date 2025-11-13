using System.ComponentModel.DataAnnotations;

namespace NoBeard.Emails.Web.Models;

public class Mail
{
    [Key]
    public int EmailId { get; set; }
    public string? MessageId { get; set; }
    public string From { get; set; } = string.Empty;
    public List<string> Tos { get; set; } = new();
    public List<string> Ccs { get; set; } = new();
    public List<string> Bccs { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTimeOffset? SendDate { get; set; }
    public EmailSendStatus SendStatus { get; set; }
    public string? SendError { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    //public List<string> Attachments { get; set; } = new();
    //public List<EmailLinkDto> EmailLinks { get; set; } // All entities this email is linked to
    //public EmailDeliveryEvent? DeliveryEvent { get; set; }

}
