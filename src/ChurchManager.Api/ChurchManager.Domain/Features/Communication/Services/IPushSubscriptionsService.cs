﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchManager.Domain.Features.Communication.Services
{
    public interface IPushSubscriptionsService
    {
        IEnumerable<PushSubscription> All();
        Task SubscribeAsync(PushSubscription subscription, string deviceType, int personId, CancellationToken ct = default);
        Task UnsubscribeAsync(PushSubscription subscription, CancellationToken ct = default);
    }
}
