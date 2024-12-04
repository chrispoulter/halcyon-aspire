namespace Halcyon.Api.Services.Email;

public class EmailSettings
{
    public static string SectionName { get; } = "Email";

    public string NoReplyAddress { get; set; }
}
