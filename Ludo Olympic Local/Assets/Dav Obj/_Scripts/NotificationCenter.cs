using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationCenter : MonoBehaviour
{

    AndroidNotificationChannel defaultchannel;

    // Start is called before the first frame update
    void Start()
    {
        defaultchannel = new AndroidNotificationChannel()
        {
            Id = "default_Channel",
            Name = "Default_Channel",
            Description = "For Generic Notifications",
            Importance = Importance.Default,
        };
        AndroidNotificationCenter.RegisterNotificationChannel(defaultchannel);
        //ShowNotification();
    }


    public void ShowNotification(int secondsss = 0, string left = "one")
    {
        string fullString;
        if (left == "one")
        {
            fullString = "one minute";
        }
        else fullString = left + " minutes";
        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Ludo Olympic",
            Text = "Your Game will start in " + fullString,
            SmallIcon = "small_icon",
            LargeIcon = "large_icon",
            FireTime = System.DateTime.Now.AddSeconds(secondsss),
        };
        
        var identifier = AndroidNotificationCenter.SendNotification(notification, "default_Channel");
    }
}
