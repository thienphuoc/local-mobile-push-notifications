#if UNITY_ANDROID
using System;
using Unity.Notifications.Android;
using UnityEngine.Assertions;

namespace Client.Notifications.Android
{
    public class AndroidGameNotification : IGameNotification
    {
        private AndroidNotification internalNotification;

        private TimeSpan cacheTimeSpan = default(TimeSpan);

        public AndroidNotification InternalNotification => internalNotification;

        public int? Id { get; set; }

        public string Title { get => InternalNotification.Title; set => internalNotification.Title = value; }
    
        public string Body { get => InternalNotification.Text; set => internalNotification.Text = value; }

        public string Subtitle { get => null; set {} }

        public string Data { get => InternalNotification.IntentData; set => internalNotification.IntentData = value; }

        public int BadgeNumber
        {
            get => internalNotification.Number;
            set => internalNotification.Number = value;
        }

        public bool ShouldAutoCancel
        {
            get => InternalNotification.ShouldAutoCancel;
            set => internalNotification.ShouldAutoCancel = value;
        }

        public DateTime DeliveryTime
        {
            get => InternalNotification.FireTime;
            set => internalNotification.FireTime = value ;
        }

        public bool Scheduled { get; private set; }

        public string SmallIcon { get => InternalNotification.SmallIcon; set => internalNotification.SmallIcon = value; }

        public string LargeIcon { get => InternalNotification.LargeIcon; set => internalNotification.LargeIcon = value; }

        public bool ShowInForeground { get; set; }

        public string ThreadIdentifierOrChannelID { get; set; }

        public TimeSpan TimeInterval
        {
            get
            {
                return cacheTimeSpan;
            }
            set
            {
                internalNotification.FireTime = DateTime.Now + value;

                cacheTimeSpan = value;
            }
        }

        public string CategoryIdentifier { get; set; }

        public AndroidGameNotification()
        {
            internalNotification = new AndroidNotification();
        }

        internal AndroidGameNotification(AndroidNotification deliveredNotification, int deliveredId,
                                         string deliveredChannel)
        {
            internalNotification = deliveredNotification;
            Id = deliveredId;
            ThreadIdentifierOrChannelID = deliveredChannel;
        }
   
        internal void OnScheduled()
        {
            Assert.IsFalse(Scheduled);
            Scheduled = true;
        }
    }
}
#endif
