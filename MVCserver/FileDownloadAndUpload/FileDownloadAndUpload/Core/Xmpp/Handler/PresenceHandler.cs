using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDownloadAndUpload;
using System.Diagnostics;

namespace FileDownloadAndUpload.Core.Xmpp.Handler
{
    public class PresenceHandler:XmppHandler
    {
        XmppServer xmppserver;
        public PresenceHandler()
            :base(typeof(agsXMPP.protocol.client.Presence))
        {
            xmppserver = XmppServer.GetInstance();
        }

        /// <summary>
        ///  route presences here and handle all subscription stuff
        /// </summary>
        /// <param name="node"></param>
        public override void Process(agsXMPP.XmppSeverConnection contextConnection,agsXMPP.Xml.Dom.Node node)
        {
            if(node.GetType()== HandlerType)
            {
                Trace.Write(node.ToString(), HandlerType.FullName);
                // route presences here and handle all subscription stuff

            }
        }
    }

}