namespace ChurchManager.Domain.Features.Communications;

public record EmailRecipient
{
    public int PersonId { get; set; }
    public string EmailAddress { get; set; }
}

public record TemplateInfo(string TemplateName, IDictionary<string, object> TemplateData);