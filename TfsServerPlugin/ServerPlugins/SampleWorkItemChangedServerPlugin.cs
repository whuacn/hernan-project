using System;
using System.Diagnostics;
using System.Text;
using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.VersionControl.Server;
using Microsoft.TeamFoundation.Client;

//Copiar en C:\Program Files\Microsoft Team Foundation Server 2010\Application Tier\Web Services\bin\Plugins
namespace NotificationSample
{
    public class CheckinSample : ISubscriber
    {
        private const string _name = "Check-in Sample Plugin";

        public string Name
        {
            get { return _name; }
        }

        public SubscriberPriority Priority
        {
            get { return SubscriberPriority.Normal; }
        }

        public EventNotificationStatus ProcessEvent(TeamFoundationRequestContext requestContext,
        NotificationType notificationType,
        object notificationEventArgs,
        out int statusCode,
        out string statusMessage,
        out ExceptionPropertyCollection properties)
        {
            statusCode = 0;
            statusMessage = string.Empty;
            properties = null;

            if (notificationType != NotificationType.Notification)
            {
                return EventNotificationStatus.ActionPermitted;
            }

            //NotificationType.Notification es cuando ya se realizó el checkin
            if (notificationType == NotificationType.Notification && notificationEventArgs is CheckinNotification)
            {
                CheckinNotification data = notificationEventArgs as CheckinNotification;
                if (data != null)
                {
                    //------------------
                    //Grabo en un txt
                    System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\test.txt");
                    file.WriteLine("Changeset: " + data.Changeset.ToString());
                    file.WriteLine("Changeset owner: " + data.ChangesetOwnerName);
                    file.WriteLine("Comment: " + data.Comment);
                    foreach (string it in data.SubmittedItems)
                    {
                        file.WriteLine("Item: " + it);            
                    }
                    file.Close();

                    //------------------
                    //Grabo en el Event Log
                    StringBuilder log = new StringBuilder();
                    log.AppendLine("Changeset: " + data.Changeset.ToString());
                    log.AppendLine("Changeset owner: " + data.ChangesetOwnerName);
                    log.AppendLine("Comment: " + data.Comment);
                    foreach (string it in data.SubmittedItems)
                    {
                        log.AppendLine("Item: " + it);
                    }
                    EventLog.WriteEntry(_name, log.ToString());

                    //------------------
                    //Copio el archivo en disco
                    foreach (string it in data.SubmittedItems)
                    {
                        var tfs = new TfsTeamProjectCollection(new Uri("http://hernandesktop:8080/tfs/tfstest"));
                        var sourceControl = requestContext.GetService<TeamFoundationVersionControlService>();
                        string filename = it.Substring(it.LastIndexOf("/") + 1);
                        sourceControl.DownloadFile(requestContext, it, 0, new ChangesetVersionSpec(data.Changeset), "C:\\UltimaVersion\\" + filename);
                    }

                }
            }
            
            return EventNotificationStatus.ActionPermitted;

        }

        public Type[] SubscribedTypes()
        {
            return new Type[] { typeof(CheckinNotification) };
        }
    }
}