using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using agsXMPP.protocol.client;
using System.Diagnostics;

namespace FileDownloadAndUpload.Core.Xmpp.Handler
{
    public class MyMessageHandler:XmppHandler
    {
        public MyMessageHandler()
            :base(typeof(agsXMPP.protocol.client.Message))
        {

        }
        public override void Process(agsXMPP.XmppSeverConnection contextConnection, agsXMPP.Xml.Dom.Node node)
        {
            if(node.GetType()==HandlerType)
            {
                Message msg = node as Message;
                if(msg.To==null)
                {

                }
                else if(msg.To.User=="0")
                {

                }
            }
        }
    }
}