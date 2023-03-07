#if UNITY_IOS
using System;
using Unity.Notifications.iOS;
using UnityEngine;
using UnityEngine.Assertions;

namespace Client.Notifications.iOS
{
    /// <summary>
    /// iOS implementation of <see cref="IGameNotification"/>.
    /// </summary>
    public class iOSGameNotification : IGameNotification
    {
        private readonly iOSNotification internalNotification;

        public iOSNotification InternalNotification => internalNotification;

        private DateTime cacheDeliveryTime;

        public int? Id
        {
            get
            {
                if (!int.TryParse(internalNotification.Identifier, out int value))
                {
                    Debug.LogWarning("Internal iOS notification's identifier isn't a number.");
                    return null;
                }

                return value;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                internalNotification.Identifier = value.Value.ToString();
            }
        }

        public string Title { get => internalNotification.Title; set => internalNotification.Title = value; }

        public string Body { get => internalNotification.Body; set => internalNotification.Body = value; }

        public string Subtitle { get => internalNotification.Subtitle; set => internalNotification.Subtitle = value; }

        public string Data { get => internalNotification.Data; set => internalNotification.Data = value; }

        public string Group { get => internalNotification.CategoryIdentifier; set => internalNotification.CategoryIdentifier = value; }

        public int BadgeNumber
        {
            get => internalNotification.Badge;
            set => internalNotification.Badge = value;
        }

        public bool ShouldAutoCancel { get; set; }

        public bool Scheduled { get; private set; }

        public string CategoryIdentifier
        {
            get => internalNotification.CategoryIdentifier;
            set => internalNotification.CategoryIdentifier = value;
        }

        public string SmallIcon { get => null; set { } }

        public string LargeIcon { get => null; set { } }

        public DateTime DeliveryTime
        {
            get
            {
                return cacheDeliveryTime;
            }

            set
            {
                var timeTrigger = new iOSNotificationTimeIntervalTrigger()
                {
                    TimeInterval = TimeInterval,
                    Repeats = false
                };

                internalNotification.Trigger = timeTrigger;

                cacheDeliveryTime = value;
            }
        }

        public bool ShowInForeground { get => internalNotification.ShowInForeground; set => internalNotification.ShowInForeground = value; }

        public PresentationOption ForegroundPresentationOption
        {
            get => internalNotification.ForegroundPresentationOption  ;

            set => internalNotification.ForegroundPresentationOption = value;
        }

        public string ThreadIdentifierOrChannelID { get => internalNotification.ThreadIdentifier; set => internalNotification.ThreadIdentifier = value; }

        public TimeSpan TimeInterval {

            get
            {
                var trigger = internalNotification.Trigger;

                if (trigger != null && trigger is iOSNotificationTimeIntervalTrigger timeIntervalTrigger) {

                   return timeIntervalTrigger.TimeInterval;
                }

                return default(TimeSpan);
            }
            set
            {
                var timeTrigger = new iOSNotificationTimeIntervalTrigger()
                {
                    TimeInterval = value,
                };

                internalNotification.Trigger = timeTrigger;
            }
        }

        public iOSGameNotification()
        {
            internalNotification = new iOSNotification()
            {
                ShowInForeground = true
            };
        }

        internal iOSGameNotification(iOSNotification internalNotification)
        {
            this.internalNotification = internalNotification;
        }

        internal void OnScheduled()
        {
            Assert.IsFalse(Scheduled);
            Scheduled = true;
        }
    }
}
#endif
