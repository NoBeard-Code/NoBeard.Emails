using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Options;
using NoBeard.Emails.Web.Models;
using NoBeard.Emails.Web.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace NoBeard.Emails.Web.Services;

public class EmailService(ILogger<EmailService> logger, IOptions<SmtpOptions> smtpOptions, IOptions<AcsOptions> acsOptions)
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;
    private readonly AcsOptions _acsOptions = acsOptions.Value;

    public async Task<Mail> SendViaSmtpAsync(Mail mail, CancellationToken cancellationToken = default)
    {
        string messageId = $"<{Guid.NewGuid()}@{_smtpOptions.DefaultFromAddress.Split("@")[1]}>";

        mail.From = _smtpOptions.DefaultFromAddress;
        mail.MessageId = messageId;
        mail.SendDate = DateTimeOffset.UtcNow;

        using SmtpClient smtpClient = new(_smtpOptions.Host, _smtpOptions.Port)
        {
            EnableSsl = _smtpOptions.EnableSsl,
            Credentials = new NetworkCredential(_smtpOptions.UserName, _smtpOptions.Password),
            Timeout = _smtpOptions.TimeoutInSeconds
        };

        var mailMessage = new MailMessage()
        {
            From = new MailAddress(mail.From),
            Subject = mail.Subject,
            Body = mail.Body,
            IsBodyHtml = true,
            BodyEncoding = Encoding.UTF8,
            SubjectEncoding = Encoding.UTF8,
            To = { new MailAddress(mail.Tos.First()) }
        };

        mailMessage.Headers.Add("Message-ID", messageId);

        try
        {
            await smtpClient.SendMailAsync(mailMessage, cancellationToken);

            mail.MessageId = messageId;
            mail.SendStatus = Models.EmailSendStatus.Sent;
            mail.SendDate = DateTimeOffset.UtcNow;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email with id: {EmailId}", mail.EmailId);

            mail.SendStatus = Models.EmailSendStatus.FailedToSend;
            mail.SendError = ex.Message;
        }

        return mail;
    }

    public async Task<Mail> SendViaAcsAsync(Mail mail, CancellationToken cancellationToken = default)
    {
        string messageId = $"<{Guid.NewGuid()}@{_smtpOptions.DefaultFromAddress.Split("@")[1]}>";

        mail.From = _acsOptions.SenderAddress;
        mail.MessageId = messageId;
        mail.SendDate = DateTimeOffset.UtcNow;

        var client = new EmailClient(_acsOptions.ConnectionString);

        var mailMessage = new EmailMessage(
            senderAddress: mail.From,
            recipientAddress: mail.Tos.First(),
            content: new EmailContent(mail.Subject)
            {
                PlainText = mail.Body,
                Html = mail.Body
            }
        );

        mailMessage.Headers.Add("Message-ID", messageId);

        var emailSendOperation = await client.SendAsync(WaitUntil.Started, mailMessage, cancellationToken: cancellationToken);

        try
        {
            while (true)
            {
                await emailSendOperation.UpdateStatusAsync(cancellationToken);
                if (emailSendOperation.HasCompleted)
                {
                    break;
                }
                await Task.Delay(100, cancellationToken);
            }

            if (emailSendOperation.HasValue)
            {
                Console.WriteLine($"Email queued for delivery. Status = {emailSendOperation.Value.Status}");
                // mail.SendStatus = emailSendOperation.Value.Status;
            }
        }
        catch (RequestFailedException ex)
        {
            Console.WriteLine($"Email send failed with Code = {ex.ErrorCode} and Message = {ex.Message}");
        }

        string operationId = emailSendOperation.Id;

        return mail;
    }
}
