using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDownloadAndUpload;
using System.Diagnostics;
using agsXMPP.protocol.client;

namespace FileDownloadAndUpload.Core.Xmpp.Handler
{
    public class PresenceHandler : XmppHandler
    {
        XmppServer xmppserver;
        public PresenceHandler()
            : base(typeof(agsXMPP.protocol.client.Presence))
        {
            xmppserver = XmppServer.GetInstance();
        }

        /// <summary>
        ///  route presences here and handle all subscription stuff
        /// </summary>
        /// <param name="node"></param>
        public override void Process(agsXMPP.XmppSeverConnection contextConnection, agsXMPP.Xml.Dom.Node node)
        {
            if (node.GetType() == HandlerType)
            {
                Trace.Write(node.ToString(), HandlerType.FullName);
                // route presences here and handle all subscription stuff
                Presence presence = node as Presence;

                ///登录
                if (presence.Type == PresenceType.subscribe)
                {
                    if (presence.Status == "online")
                    {
                        int uid = Convert.ToInt32(presence.From.User);
                        if (xmppserver.XmppConnectionDic.ContainsKey(uid))
                        {
                            xmppserver.XmppConnectionDic.Remove(uid);
                        }
                        xmppserver.XmppConnectionDic.Add(uid, contextConnection);
                        Presence reply = new Presence();
                        reply.From = ServerJid;
                        reply.To = presence.From;
                        reply.Status = "onlined";
                        reply.Type = PresenceType.subscribed;
                        reply.Id = presence.Id;
                        contextConnection.Send(reply);
                        xmppserver.Broadcast(presence);
                    }
                 

                }
                    ///注销用户
                else if (presence.Type == PresenceType.unsubscribe)
                {
                    int uid = Convert.ToInt32(presence.From.User);
                    if (xmppserver.XmppConnectionDic.ContainsKey(uid))
                    {
                        xmppserver.XmppConnectionDic.Remove(uid);
                    }
                    xmppserver.Broadcast(presence);
                    presence.Type =PresenceType.unsubscribed;
                    contextConnection.Send(presence);


                }
                else if(presence.Type == PresenceType.available)
                {
                    if (presence.Status == "ping")
                    {
                        presence.Type = PresenceType.available;
                        presence.Status = "pinged";
                        contextConnection.Send(presence);
                    }
                }


            }
        }
    }

}