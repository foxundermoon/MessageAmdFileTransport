﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using agsXMPP.protocol.client;
using System.Diagnostics;

namespace FileDownloadAndUpload.Core.Xmpp
{
    public partial class Xmppserver
    {
        private void processMyMessage(agsXMPP.XmppSeverConnection contextConnection, Message msg)
        {
            {
                if (msg.To == null)
                {
                    //to do sth
                }
                else if (msg.To.User == "0")
                {
                    //todo sth

                }
            }
        }
    }
}