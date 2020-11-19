using BellumGens.Api.Core.Models;
using System.Collections.Generic;

namespace BellumGens.Api.Core.Providers
{
    public interface INotificationService
    {
        public void SendNotification(List<BellumGensPushSubscription> subs, TeamInvite notification);
        public void SendNotification(List<BellumGensPushSubscription> subs, TeamInvite notification, NotificationState state);
        public void SendNotification(List<BellumGensPushSubscription> subs, TeamApplication notification);
        public void SendNotification(List<BellumGensPushSubscription> subs, TeamApplication notification, NotificationState state);
        public void SendNotification(List<BellumGensPushSubscription> subs, StrategyComment comment);

    }
}
