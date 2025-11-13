namespace NoBeard.Emails.Web.Options;

public class AcsOptions
{
    public const string Section = "Acs";

    public string ConnectionString { get; set; }

    public string SenderAddress { get; set; }
}
