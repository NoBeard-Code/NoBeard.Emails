namespace NoBeard.Emails.Web.Options;

public record SmtpOptions
{
    public const string Section = "Smtp";

    public string Host { get; set; }

    public int Port { get; set; }

    public bool EnableSsl { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string DefaultFromAddress { get; set; }

    public int TimeoutInSeconds { get; set; }
}