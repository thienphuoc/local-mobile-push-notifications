#if UNITY_ANDROID
using UnityEngine;

namespace Client.Notifications.Android
{
    public class AndroidNotificationCenterExtensions
    {
        private static bool s_Initialized;
        private static AndroidJavaObject s_AndroidNotificationExtensions;

        public static bool Initialize()
        {
            if (s_Initialized)
            {
                return true;
            }

#if UNITY_EDITOR
            s_AndroidNotificationExtensions = null;
            s_Initialized = false;
#else
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

            AndroidJavaClass managerClass = new AndroidJavaClass("com.unity.androidnotifications.UnityNotificationManager");
            AndroidJavaObject notificationManagerImpl = managerClass.CallStatic<AndroidJavaObject>("getNotificationManagerImpl", context, activity);
            AndroidJavaObject notificationManager = notificationManagerImpl.Call<AndroidJavaObject>("getNotificationManager");

            AndroidJavaClass pluginClass = new AndroidJavaClass("com.unity.androidnotifications.AndroidNotificationCenterExtensions");
            s_AndroidNotificationExtensions = pluginClass.CallStatic<AndroidJavaObject>("getExtensionsImpl", context, notificationManager);

            s_Initialized = true;
#endif
            return s_Initialized;
        }

        public static bool AreNotificationsEnabled()
        {
            if (!s_Initialized)
            {
                // By default notifications are enabled
                return true;
            }

            return s_AndroidNotificationExtensions.Call<bool>("areNotificationsEnabled");
        }

        public static bool AreNotificationsEnabled(string channelId)
        {
            if (!s_Initialized)
            {
                // By default notifications are enabled
                return true;
            }

            return s_AndroidNotificationExtensions.Call<bool>("areNotificationsEnabled", channelId);
        }
    }
}
#endif
