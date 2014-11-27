using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using agsXMPP.protocol.client;
using agsXMPP.protocol;
using FileDownloadAndUpload;
using FileDownloadAndUpload.Core.Common;
using System.Diagnostics;
using FileDownloadAndUpload.Core.Xmpp;
using agsXMPP;
namespace FileDownloadAndUpload.Core.Xmpp.Handler
{

    public class MessageHandler : XmppHandler
    {
        XmppServer xmppserver;
        Models.MessageEntities1 messageEntities;
        public MessageHandler()
            : base(typeof(Message))
        {
            xmppserver = XmppServer.GetInstance();
            messageEntities = MessageEntityFictory.ModelsEntities;
        }

        /// <summary>
        /// 处理xmpp消息的方法
        /// </summary>
        /// <param name="node">xml解析出的node</param>
        public override void Process(agsXMPP.XmppSeverConnection contextConnection, agsXMPP.Xml.Dom.Node node)
        {

            if (node.GetType() == HandlerType)
            {
                Trace.Write(node.ToString(), HandlerType.FullName);
                Message msg = node as Message;
                Models.Message dbmsg = messageEntities.Message.Create();
                dbmsg.Mid = msg.Id;
                //dbmsg.Status = 0;
                dbmsg.Content = msg.Body;


                if (msg.To != null)
                {
                    int uid = 0;
                    try
                    {
                        uid = int.Parse(msg.To.User);
                        dbmsg.To = uid;


                    }
                    catch (FormatException fe)
                    {
                        Trace.Write(fe.Message, fe.GetType().FullName);
                        //throw fe;
                    }

                    if (msg.From != null)
                    {
                        dbmsg.Resource = msg.From.Resource;
                        int fromUid = -1;
                        try
                        {
                            fromUid = int.Parse(msg.From.User);
                            dbmsg.From = fromUid;

                        }
                        catch (FormatException fe)
                        {
                            Trace.Write("format  the user id of From catch Exception \n:" + fe.Message, fe.GetType().FullName);
                        }


                    }
                    // To =0  send to server,  add the connenction to dic
                    if (dbmsg.To.HasValue && dbmsg.To == 0)
                    {
                        if (dbmsg.From.HasValue)
                        {
                            if (!xmppserver.XmppConnectionDic.ContainsKey(dbmsg.From.Value))
                            {
                                xmppserver.XmppConnectionDic.Add(dbmsg.From.Value, contextConnection);
                            }
                        }
                    }
                    //转发 message
                    if (dbmsg.To.HasValue && dbmsg.To > 0)
                    {
                        XmppSeverConnection connection;
                        if (xmppserver.XmppConnectionDic.TryGetValue(dbmsg.To.Value, out connection))
                        {
                            connection.Send(msg);
                        }
                    }

                }
                messageEntities.Message.Add(dbmsg);
                try
                {
                    messageEntities.SaveChanges();

                }
                catch (Exception e)
                {
                    Trace.Write("save message to database catch exception :" + e.Message, "messageEntities.SaveChanges()");
                }

            }
        }
    }
}