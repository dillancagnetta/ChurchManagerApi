using ChurchManager.Domain.Features.Groups.Extensions;
using ChurchManager.Domain.Features.People.Extensions;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Communications.Extensions;

public static class CommunicationsModelExtensions
{
    public static CommunicationViewModel ToViewModel(this Communication model)
    {
        return new CommunicationViewModel
        {
            Id = model.Id,
            Name = model.Name!,
            Subject = model.Subject,
            Category = model.Category,
            Content = model.CommunicationContent!,
            CommunicationType = model.CommunicationType.Value,
            Status = model.Status.Value,
            ListGroup = model.ListGroup.ToReference(),
            CommunicationTemplateId = model.CommunicationTemplateId,
            IsBulkCommunication = model.IsBulkCommunication,
            RecipientCount = model.Recipients?.Count ?? 0,
            CommunicationContent = model.CommunicationContent,
            SenderPerson = model.SenderPerson.ToBasicPersonViewModel(),
        };
    }
    
    public static CommunicationRecipientViewModel ToViewModel(this CommunicationRecipient model)
    {
        return new CommunicationRecipientViewModel
        {
            Id = model.Id,
            RecipientPerson = model.RecipientPerson.ToBasicPersonViewModel(),
            Status = model.Status.Value,
            StatusNote = model.StatusNote,
            SendDateTime = model.SendDateTime,
            OpenedDateTime = model.OpenedDateTime,
            UniqueMessageId = model.UniqueMessageId,
            AttemptCount = model.AttemptCount,
        };
    }
}