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
namespace FileDownloadAndUpload.Core.Xmpp
{
    public partial class XmppServer
    {
        public void OnMessage(XmppSeverConnection contextConnection, Message message)
        {
            if (contextConnection.IsAuthentic)
            {
                processMessage(contextConnection, message);

            }
            else
            {
                contextConnection.Stop();
            }

        }
        private  void processMessage(agsXMPP.XmppSeverConnection contextConnection, Message msg)
        {
            Models.Message dbmsg = entities.Message.Create();
            //Trace.Write(msg.ToString(),msg.GetType().ToString);
            //dbmsg.Status = 0;
            dbmsg.Mid = msg.Id;
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
                    //Trace.Write(fe.Message, fe.GetType().FullName);
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
                        if (!XmppConnectionDic.ContainsKey(dbmsg.From.Value))
                        {
                            XmppConnectionDic.Add(dbmsg.From.Value, contextConnection);
                        }
                    }
                }
                //转发 message
                if (dbmsg.To.HasValue && dbmsg.To > 0)
                {
                    XmppSeverConnection connection;
                    if (XmppConnectionDic.TryGetValue(dbmsg.To.Value, out connection))
                    {
                        connection.Send(msg);
                    }
                }

            }
            entities.Message.Add(dbmsg);
            try
            {
                entities.SaveChanges();
            }
            catch (Exception e)
            {
                Trace.Write("save message to database catch exception :" + e.Message, "messageEntities.SaveChanges()");
            }

        }
    }
}
