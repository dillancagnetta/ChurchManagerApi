namespace ChurchManager.Domain.Features.Communication;

public record EmailRecipient
{
    public int PersonId { get; set; }
    public string EmailAddress { get; set; }
}