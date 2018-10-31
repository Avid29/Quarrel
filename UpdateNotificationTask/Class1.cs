﻿using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace UpdateNotificationTask
{
    public sealed class UpdateTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            ToastContent content = new ToastContent();
            ToastVisual visual = new ToastVisual()
            {
BindingGeneric=    new ToastBindingGeneric()
            {
                Children =
    {
        new AdaptiveText()
        {
            Text = "Quarrel has been updated!"
        },

        new AdaptiveText()
        {
            Text = "Full Voice support and myPeople integration with version 10.1!"
        }
                }
                }
            };
            content.Visual = visual;
            var toast = new ToastNotification(content.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);

            //now change the responsive UI breakpoints
            try
            {
            }
            catch
            {
               //well, they weren't touched
            }
        }

    }
}
