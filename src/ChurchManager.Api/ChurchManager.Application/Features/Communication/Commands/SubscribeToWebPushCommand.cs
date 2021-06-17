﻿using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Common;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Services;
using MediatR;

namespace ChurchManager.Application.Features.Communication.Commands
{
    public record SubscribeToWebPushCommand(PushSubscription Subscription) : IRequest
    {
        public string Device { get; set; }
    }

    public record UnsubscribeToWebPushCommand(PushSubscription Subscription) : IRequest
    {
    }

    public class WebPushSubscriptionHandler : IRequestHandler<SubscribeToWebPushCommand>
    {
        private readonly ICognitoCurrentUser _currentUser;
        private readonly IPushSubscriptionsService _push;

        public WebPushSubscriptionHandler(ICognitoCurrentUser currentUser, IPushSubscriptionsService push)
        {
            _currentUser = currentUser;
            _push = push;
        }

        public async Task<Unit> Handle(SubscribeToWebPushCommand command, CancellationToken ct)
        {
            await _push.SubscribeAsync(command.Subscription, command.Device, _currentUser.PersonId, ct);

            return new Unit();
        }
    }

    public class WebPushUnsubscribeHandler : IRequestHandler<UnsubscribeToWebPushCommand>
    {
        private readonly IPushSubscriptionsService _push;

        public WebPushUnsubscribeHandler(IPushSubscriptionsService push)
        {
            _push = push;
        }

        public async Task<Unit> Handle(UnsubscribeToWebPushCommand command, CancellationToken ct)
        {
            await _push.UnsubscribeAsync(command.Subscription, ct);

            return new Unit();
        }
    }
}