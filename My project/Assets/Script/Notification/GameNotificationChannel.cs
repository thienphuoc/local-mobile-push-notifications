using System;
using System.Linq;

namespace Client.Notifications
{

    public struct GameNotificationChannel
    {
       
        public enum NotificationStyle
        {
            /// <summary>
            /// Notification does not appear in the status bar.
            /// </summary>
            None = 0,
            /// <summary>
            /// Notification makes no sound.
            /// </summary>
            NoSound = 2,
            /// <summary>
            /// Notification plays sound.
            /// </summary>
            Default = 3,
            /// <summary>
            /// Notification also displays a heads-up popup.
            /// </summary>
            Popup = 4
        }

        /// <summary>
        /// Controls how notifications display on the device lock screen.
        /// </summary>
        public enum PrivacyMode
        {
            /// <summary>
            /// Notifications aren't shown on secure lock screens.
            /// </summary>
            Secret = -1,
            /// <summary>
            /// Notifications display an icon, but content is concealed on secure lock screens.
            /// </summary>
            Private = 0,
            /// <summary>
            /// Notifications display on all lock screens.
            /// </summary>
            Public
        }

        public readonly string Id;

        public readonly string Name;

        public readonly string Description;

        public readonly bool ShowsBadge;

        public readonly bool ShowLights;

        public readonly bool Vibrates;

        public readonly bool HighPriority;

        public readonly NotificationStyle Style;
  
        public readonly PrivacyMode Privacy;

        public readonly int[] VibrationPattern;

        public GameNotificationChannel(string id, string name, string description) : this()
        {
            Id = id;
            Name = name;
            Description = description;
            ShowsBadge = true;
            ShowLights = false;
            Vibrates = true;
            HighPriority = false;
            Style = NotificationStyle.Popup;
            Privacy = PrivacyMode.Public;
            VibrationPattern = null;
        }

        public GameNotificationChannel(string id, string name, string description, NotificationStyle style, bool showsBadge = true, bool showLights = false, bool vibrates = true, bool highPriority = false, PrivacyMode privacy = PrivacyMode.Public, long[] vibrationPattern = null)
        {
            Id = id;
            Name = name;
            Description = description;
            ShowsBadge = showsBadge;
            ShowLights = showLights;
            Vibrates = vibrates;
            HighPriority = highPriority;
            Style = style;
            Privacy = privacy;
            if (vibrationPattern != null)
                VibrationPattern = vibrationPattern.Select(v => (int)v).ToArray();
            else
                VibrationPattern = null;
        }
    }
}
