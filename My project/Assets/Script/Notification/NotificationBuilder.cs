using System;
using Client.Notifications;

public class NotificationBuilder
{
    private IGameNotification _notification = null;

    public IGameNotification Notification => _notification;

    public NotificationBuilder(IGameNotification notification)
    {
        _notification = notification ?? throw new ArgumentNullException(nameof(notification));
    }

    public NotificationBuilder SetTitle(string title)
    {
        _notification.Title = title;

        return this;
    }

    public NotificationBuilder SetBody(string body)
    {
        _notification.Body = body;

        return this;
    }
    public NotificationBuilder SetSubtitle(string subtitle)
    {
        _notification.Subtitle = subtitle;

        return this;
    }

    public NotificationBuilder SetDeliveryTime(DateTime deliveryTime)
    {
        _notification.DeliveryTime = deliveryTime;

        return this;
    }

    public NotificationBuilder SetTimeInterval(TimeSpan timeSpan)
    {
        _notification.TimeInterval = timeSpan;

        return this;
    }

    public NotificationBuilder SetSmallIcon(string smallIcon)
    {
        _notification.SmallIcon = smallIcon;

        return this;
    }

    public NotificationBuilder SetLargeIcon(string largeIcon)
    {
        _notification.LargeIcon = largeIcon;

        return this;
    }

    public NotificationBuilder SetBadgeNumber(int badgeNumber)
    {
        _notification.BadgeNumber = badgeNumber;

        return this;
    }

    public NotificationBuilder SetShowInForeground(bool isShow)
    {
        _notification.ShowInForeground = isShow;

        return this;
    }

    public NotificationBuilder SetCategoryIdentifier(string categoryIdentifier)
    {
        _notification.CategoryIdentifier = categoryIdentifier;

        return this;
    }

    public NotificationBuilder SetThreadIdOrChanelID(string threadIdOrChanelID)
    {
        _notification.ThreadIdentifierOrChannelID = threadIdOrChanelID;

        return this;
    }

}