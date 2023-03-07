using System;
using System.Collections;
using System.Collections.Generic;
using Client.Notifications;
using UnityEngine;

public class NotificationControllerExample : MonoBehaviour
{
    private GameNotificationsManager gameManager = default;

    private static string NOTIFICATION_THREAD_ID = "game_notification_id";

    void Start()
    {
        gameManager = new GameNotificationsManager();

        var registerAndroidChannel = new GameNotificationChannel(NOTIFICATION_THREAD_ID, "Default Game Channel", "Generic notifications");

        gameManager.Initialize(registerAndroidChannel);
    }

    public void OnNotificationClick()
    {
        var builder = gameManager.CreateNotification();

        builder.SetTitle("Title")
            .SetBody("Scheduled at: " + DateTime.Now.ToShortDateString() + " triggered in 5 seconds")
            .SetSubtitle("This is a subtitle, something, something important...")
            .SetTimeInterval(new TimeSpan(0, 0, 15))
            .SetCategoryIdentifier("MESSAGE_CATEGORY")
            .SetThreadIdOrChanelID(NOTIFICATION_THREAD_ID)
            .SetSmallIcon("icon_0")
            .SetLargeIcon("icon_1")
            .SetBadgeNumber(1);

        gameManager.ScheduleNotification(builder.Notification);
    }

}
