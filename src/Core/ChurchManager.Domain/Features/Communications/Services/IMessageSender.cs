namespace ChurchManager.Domain.Features.Communications.Services;

public interface IMessageSender
{
    Task SendAsync(Message message, CancellationToken ct = default);
}