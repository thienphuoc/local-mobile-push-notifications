using System;

namespace Client.Notifications
{
    public interface IGameNotification
    {
        int? Id { get; set; }

        string Title { get; set; }

        string Body { get; set; }

        string Subtitle { get; set; }

        string Data { get; set; }

        int BadgeNumber { get; set; }

        bool ShouldAutoCancel { get; set; }

        DateTime DeliveryTime { get; set; }

        bool Scheduled { get; }

        string SmallIcon { get; set; }

        string LargeIcon { get; set; }

        bool ShowInForeground { get; set; }

        string ThreadIdentifierOrChannelID { get; set; }

        TimeSpan TimeInterval { get; set; }

        string CategoryIdentifier { get; set; }
    }
}
