using System;
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

namespace FileDownloadAndUpload.Core.Xmpp.Handler
{
    public class IQHandler:XmppHandler
    {
        Models.MessageEntities1 entities = Core.Common.MessageEntityFictory.ModelsEntities;
        XmppServer xmppserver = XmppServer.GetInstance();

        public IQHandler()
            :base(typeof(agsXMPP.protocol.client.IQ))
        {
            
        }
        public override void Process(agsXMPP.XmppSeverConnection contextConnection,agsXMPP.Xml.Dom.Node node)
        {
            if(node.GetType()==HandlerType)
            {
                Trace.Write(node, HandlerType.FullName);
                ProcessIQ(contextConnection, node as IQ);
            }
        }
              private void ProcessIQ(agsXMPP.XmppSeverConnection contextConnection,IQ iq)
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
                        auth.AddChild(new Element("digest"));
                        contextConnection.Send(iq);
                        break;
                    case IqType.set:
                        // Here we should verify the authentication credentials
                        iq.SwitchDirection();
                        if(AccountBus.CheckAccount(auth.Username,auth.Password))  //验证用户是否存在或者密码是否正确
                        {
                             iq.Type = IqType.result;
                             iq.Query = null;
                            try
                            {
                                int uid = int.Parse(iq.From.User);
                                if(!xmppserver.XmppConnectionDic.ContainsKey(uid))
                                {
                                    xmppserver.XmppConnectionDic.Add(uid,contextConnection);
                                }

                            }catch(Exception e)
                            {
                                // 消息没有 From    dosomething
                            }

                        }
                        else
                        {
                        //iq.Type = IqType.error;  //若要开启验证功能去掉此注释
                             iq.Type = IqType.result;
                             iq.Query = null;
                        }
                        contextConnection.Send(iq);
                        break;
                }

            }
            else if (iq.Query.GetType() == typeof(Roster))
            {
                ProcessRosterIQ(contextConnection,iq);

            }

        }

        private void ProcessRosterIQ(agsXMPP.XmppSeverConnection contextConnection,IQ iq)
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
