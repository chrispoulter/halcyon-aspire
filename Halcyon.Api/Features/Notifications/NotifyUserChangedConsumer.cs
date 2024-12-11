﻿using Halcyon.Api.Data.Users;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Halcyon.Api.Features.Notifications;

public class NotifyUserCreatedConsumer(
    IHubContext<NotificationHub, INotificationClient> eventHubContext,
    ILogger<NotifyUserCreatedConsumer> logger
) : IConsumer<Batch<UserCreatedDomainEvent>>
{
    public async Task Consume(ConsumeContext<Batch<UserCreatedDomainEvent>> context)
    {
        foreach (var message in context.Message.Select(m => m.Message))
        {
            var notification = new Notification("UserCreated", new { message.Id });

            var groups = new[]
            {
                NotificationHub.GetGroupForRole(Roles.SystemAdministrator),
                NotificationHub.GetGroupForRole(Roles.UserAdministrator),
                NotificationHub.GetGroupForUser(message.Id),
            };

            logger.LogInformation(
                "Sending notification for {Notification} to {Groups}, UserId: {UserId}",
                notification.Type,
                groups,
                message.Id
            );

            await eventHubContext
                .Clients.Groups(groups)
                .ReceiveNotification(notification, context.CancellationToken);
        }
    }
}
