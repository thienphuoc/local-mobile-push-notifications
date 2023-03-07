#if UNITY_ANDROID
using System;
using Unity.Notifications.Android;

namespace Client.Notifications.Android
{
    public class AndroidNotificationsPlatform : IGameNotificationsPlatform<AndroidGameNotification>,
        IDisposable
    {
        public event Action<IGameNotification> NotificationReceived;

        public string DefaultChannelId { get; set; }

        public AndroidNotificationsPlatform()
        {
            AndroidNotificationCenter.OnNotificationReceived += OnLocalNotificationReceived;
        }

        public void ScheduleNotification(AndroidGameNotification gameNotification)
        {
            if (gameNotification == null)
            {
                throw new ArgumentNullException(nameof(gameNotification));
            }

            if (gameNotification.Id.HasValue)
            {
                AndroidNotificationCenter.SendNotificationWithExplicitID(gameNotification.InternalNotification,
                    gameNotification.ThreadIdentifierOrChannelID,
                    gameNotification.Id.Value);
            }
            else
            {
                int notificationId = AndroidNotificationCenter.SendNotification(gameNotification.InternalNotification,
                    gameNotification.ThreadIdentifierOrChannelID);
                gameNotification.Id = notificationId;
            }

            gameNotification.OnScheduled();
        }

        public void ScheduleNotification(IGameNotification gameNotification)
        {
            if (gameNotification == null)
            {
                throw new ArgumentNullException(nameof(gameNotification));
            }

            if (!(gameNotification is AndroidGameNotification androidNotification))
            {
                throw new InvalidOperationException(
                    "Notification provided to ScheduleNotification isn't an AndroidGameNotification.");
            }

            ScheduleNotification(androidNotification);
        }

        public AndroidGameNotification CreateNotification()
        {
            var notification = new AndroidGameNotification()
            {
                ThreadIdentifierOrChannelID = DefaultChannelId
            };

            return notification;
        }

        IGameNotification IGameNotificationsPlatform.CreateNotification()
        {
            return CreateNotification();
        }

        public void CancelNotification(int notificationId)
        {
            AndroidNotificationCenter.CancelScheduledNotification(notificationId);
        }

        public void DismissNotification(int notificationId)
        {
            AndroidNotificationCenter.CancelDisplayedNotification(notificationId);
        }

        public void CancelAllScheduledNotifications()
        {
            AndroidNotificationCenter.CancelAllScheduledNotifications();
        }

        public void DismissAllDisplayedNotifications()
        {
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
        }

        IGameNotification IGameNotificationsPlatform.GetLastNotification()
        {
            return GetLastNotification();
        }

        public AndroidGameNotification GetLastNotification()
        {
            var data = AndroidNotificationCenter.GetLastNotificationIntent();

            if (data != null)
            {
                return new AndroidGameNotification(data.Notification, data.Id, data.Channel);
            }

            return null;
        }

        public void OnForeground() {}

    
        public void OnBackground() {}


        public void Dispose()
        {
            AndroidNotificationCenter.OnNotificationReceived -= OnLocalNotificationReceived;
        }

        private void OnLocalNotificationReceived(AndroidNotificationIntentData data)
        {
            NotificationReceived?.Invoke(new AndroidGameNotification(data.Notification, data.Id, data.Channel));
        }
    }
}
#endif
