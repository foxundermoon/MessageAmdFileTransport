﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDownloadAndUpload;
using System.Diagnostics;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.auth;
using agsXMPP.Xml.Dom;
using FileDownloadAndUpload.Core.Account;
using agsXMPP.protocol.iq.roster;
using agsXMPP;
using FileDownloadAndUpload.Models;

namespace FileDownloadAndUpload.Core.Xmpp
{
    public partial class XmppServer
    {
        public void OnIQ(XmppSeverConnection contextConnection, IQ iq)
        {
            ProcessIQ(contextConnection, iq);
        }
        private void ProcessIQ(agsXMPP.XmppSeverConnection contextConnection, IQ iq)
        {
            if (iq.Query.GetType() == typeof(Auth))
            {
                Auth auth = iq.Query as Auth;
                switch (iq.Type)
                {
                    case IqType.get:
                        iq.SwitchDirection();
                        iq.Type = IqType.result;
                        auth.AddChild(new Element("password"));
                        //auth.AddChild(new Element("digest"));
                        contextConnection.Send(iq);
                        break;
                    case IqType.set:
                        // Here we should verify the authentication credentials
                        iq.SwitchDirection();
                        if (AccountBus.CheckAccount(auth.Username, auth.Password))  //验证用户是否存在或者密码是否正确
                        {
                            contextConnection.IsAuthentic = true;
                            iq.Type = IqType.result;
                            iq.Query = null;
                            try
                            {
                                int uid = int.Parse(auth.Username);
                                if (XmppConnectionDic.ContainsKey(uid))
                                {
                                    XmppConnectionDic.Remove(uid);
                                }
                                XmppConnectionDic.Add(uid, contextConnection);
                            }
                            catch (Exception e)
                            {
                                // 消息没有 From    dosomething
                                iq.Type = IqType.error;
                                iq.Value = e.Message;
                            }
                        }
                        else
                        {
                            // authorize failed
                            iq.Type = IqType.error;  //若要开启验证功能去掉此注释
                            //iq.Type = IqType.result;
                            iq.Query = null;
                            iq.Value = "authorized failed";
                            contextConnection.IsAuthentic = false;
                        }
                        contextConnection.Send(iq);
                        break;
                }
            }
            else if (!contextConnection.IsAuthentic)
            {
                contextConnection.Stop();
            }

            else if (iq.Query.GetType() == typeof(Roster))
            {
                ProcessRosterIQ(contextConnection, iq);

            }

        }

        private void ProcessRosterIQ(agsXMPP.XmppSeverConnection contextConnection, IQ iq)
        {
            if (iq.Type == IqType.get)
            {
                // Send the roster
                // we send a dummy roster here, you should retrieve it from a
                // database or some kind of directory (LDAP, AD etc...)
                iq.SwitchDirection();
                iq.Type = IqType.result;
                for (int i = 1; i < 11; i++)
                {
                    RosterItem ri = new RosterItem();
                    ri.Name = "Item " + i.ToString();
                    ri.Subscription = SubscriptionType.both;
                    ri.Jid = new Jid("item" + i.ToString() + "@localhost");
                    ri.AddGroup("localhost");
                    iq.Query.AddChild(ri);
                }

                RosterItem ri1 = new RosterItem();

                for (int i = 1; i < 11; i++)
                {
                    RosterItem ri = new RosterItem();
                    ri.Name = "Item JO " + i.ToString();
                    ri.Subscription = SubscriptionType.both;
                    ri.Jid = new Jid("item" + i.ToString() + "@jabber.org");
                    ri.AddGroup("JO");
                    iq.Query.AddChild(ri);
                }
                contextConnection.Send(iq);
            }
        }
    }
}
