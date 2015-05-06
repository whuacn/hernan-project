﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Outlook;
using Google.Apis.Calendar.v3.Data;

namespace OutlookGoogleCalendarSync {
    public interface OutlookInterface {
        void Connect();
        List<String> Accounts();
        Dictionary<string, MAPIFolder> CalendarFolders();
        MAPIFolder UseOutlookCalendar();
        void UseOutlookCalendar(MAPIFolder set);
        String CurrentUserSMTP();
        String CurrentUserName();
        Boolean Offline();

        String GetRecipientEmail(Recipient recipient);
        OlExchangeConnectionMode ExchangeConnectionMode();
    }
}
