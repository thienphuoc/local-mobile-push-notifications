using System;
using System.Linq;

#if UNITY_ANDROID
using Unity.Notifications.Android;
using Client.Notifications.Android;
#elif UNITY_IOS
using Client.Notifications.iOS;
#endif

namespace Client.Notifications
{
    public class GameNotificationsManager 
    {
        public IGameNotificationsPlatform Platform { get; private set; }

        public bool Initialized { get; private set; }

        private bool inForeground = true;

        public void Initialize(params GameNotificationChannel[] channels)
        {
            if (Initialized)
            {
                throw new InvalidOperationException("NotificationsManager already initialized.");
            }

            Initialized = true;

#if UNITY_ANDROID

            Platform = new AndroidNotificationsPlatform();

            var doneDefault = false;

            foreach (GameNotificationChannel notificationChannel in channels)
            {
                if (!doneDefault)
                {
                    doneDefault = true;
                    ((AndroidNotificationsPlatform)Platform).DefaultChannelId = notificationChannel.Id;
                }

                long[] vibrationPattern = null;
                if (notificationChannel.VibrationPattern != null)
                    vibrationPattern = notificationChannel.VibrationPattern.Select(v => (long)v).ToArray();

                var androidChannel = new AndroidNotificationChannel(notificationChannel.Id, notificationChannel.Name,
                    notificationChannel.Description,
                    (Importance)notificationChannel.Style)
                {
                    CanBypassDnd = notificationChannel.HighPriority,
                    CanShowBadge = notificationChannel.ShowsBadge,
                    EnableLights = notificationChannel.ShowLights,
                    EnableVibration = notificationChannel.Vibrates,
                    LockScreenVisibility = (LockScreenVisibility)notificationChannel.Privacy,
                    VibrationPattern = vibrationPattern
                };

                AndroidNotificationCenter.RegisterNotificationChannel(androidChannel);
            }
#elif UNITY_IOS
            Platform = new iOSNotificationsPlatform();
#endif

            if (Platform == null)
            {
                return;
            }

            OnForegrounding();
        }

        protected virtual void OnDestroy()
        {
            if (Platform == null)
            {
                return;
            }

            Platform.NotificationReceived -= OnNotificationReceived;

            if (Platform is IDisposable disposable)
            {
                disposable.Dispose();
            }

            inForeground = false;
        }

        protected void OnApplicationFocus(bool hasFocus)
        {
            if (Platform == null || !Initialized)
            {
                return;
            }

            inForeground = hasFocus;

            if (hasFocus)
            {
                OnForegrounding();

                return;
            }

            Platform.OnBackground();
        }

        public NotificationBuilder CreateNotification()
        {
            if (!Initialized)
            {
                throw new InvalidOperationException("Must call Initialize() first.");
            }

            var notification = Platform?.CreateNotification();

            if(notification == null)
            {
                return null;
            }

            NotificationBuilder notificationBuilder = new NotificationBuilder(notification);

            return notificationBuilder;
        }

        public void ScheduleNotification(IGameNotification notification)
        {
            if (!Initialized)
            {
                throw new InvalidOperationException("Must call Initialize() first.");
            }

            if (notification == null || Platform == null)
            {
                return;
            }

            if (!notification.Id.HasValue)
            {
                notification.Id = Math.Abs(DateTime.Now.ToString("yyMMddHHmmssffffff").GetHashCode()); ;
            }

            Platform.ScheduleNotification(notification);
        }

        public void CancelNotification(int notificationId)
        {
            if (!Initialized)
            {
                throw new InvalidOperationException("Must call Initialize() first.");
            }

            if (Platform == null)
            {
                return;
            }

            Platform.CancelNotification(notificationId);
        }

        public void CancelAllNotifications()
        {
            if (!Initialized)
            {
                throw new InvalidOperationException("Must call Initialize() first.");
            }

            if (Platform == null)
            {
                return;
            }

            Platform.CancelAllScheduledNotifications();
        }

        public void DismissNotification(int notificationId)
        {
            if (!Initialized)
            {
                throw new InvalidOperationException("Must call Initialize() first.");
            }

            Platform?.DismissNotification(notificationId);
        }
 
        public void DismissAllNotifications()
        {
            if (!Initialized)
            {
                throw new InvalidOperationException("Must call Initialize() first.");
            }

            Platform?.DismissAllDisplayedNotifications();
        }

        public IGameNotification GetLastNotification()
        {
            if (!Initialized)
            {
                throw new InvalidOperationException("Must call Initialize() first.");
            }

            return Platform?.GetLastNotification();
        }

        private void OnNotificationReceived(IGameNotification deliveredNotification)
        {
            if (!inForeground)
            {
                return;
            }
        }

        private void OnForegrounding()
        {
            Platform.OnForeground();
        }
    }

}
