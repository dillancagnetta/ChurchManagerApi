namespace ChurchManager.Domain.Features.Communication.Services;

public interface IMessageSender
{
    Task SendAsync(Message message, CancellationToken ct = default);
}