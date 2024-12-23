﻿using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Communication;
using MediatR;

namespace ChurchManager.Features.Communication.Commands
{
    public record SendWebPushNotificationCommand(int PersonId, string Payload) : IRequest<Unit>
    {
    }

    public class SendWebPushHandler : IRequestHandler<SendWebPushNotificationCommand, Unit>
    {
        private readonly IPushNotificationService _push;

        public SendWebPushHandler(IPushNotificationService push)
        {
            _push = push;
        }

        public async Task<Unit> Handle(SendWebPushNotificationCommand command, CancellationToken ct)
        {
            var notification = new PushNotification(command.Payload);

            await _push.SendNotificationToPersonAsync(command.PersonId, notification, ct);

            return Unit.Value;
        }
    }
}