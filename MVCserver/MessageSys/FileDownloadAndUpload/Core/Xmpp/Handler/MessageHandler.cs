using System;
//using System.Collections.Generic;
using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol;
using FileDownloadAndUpload;
using System.Diagnostics;
using FileDownloadAndUpload.Core.Xmpp;
using agsXMPP;
using FileDownloadAndUpload.Core.Utils;
using System.Data.Entity.Validation;
namespace FileDownloadAndUpload.Core.Xmpp {
    public partial class XmppServer {
        public void OnMessage( XmppSeverConnection contextConnection, Message message ) {
            if(contextConnection.IsAuthentic) {
                processMessage(contextConnection, message);

            } else {
                contextConnection.Stop();
            }
        }
        private async void processMessage( agsXMPP.XmppSeverConnection contextConnection, Message msg ) {
            var content="";
            var from="";
            var to="";
            var resource="";


            if(!string.IsNullOrEmpty(msg.Language) && msg.Language.ToUpper().Contains("BASE64")) {
                content= EncryptUtil.DecryptBASE64ByGzip(msg.Body);
            } else {
                content= msg.Body;
                //dbmsg.Content = msg.Body;
            }
            if(msg.To != null) {
                to=msg.To.User;
            }

            if(msg.From != null) {
                resource= msg.From.Resource;
                from = msg.From.User;
            }
            // To =0  send to server,  add the connenction to dic
            //if(to == 0) {
            //    if(from!=-1) {
            //        msg.SwitchDirection();
            //        contextConnection.Send(msg);

            //        if(!XmppConnectionDic.ContainsKey(dbmsg.From.Value)) {
            //            XmppConnectionDic.Add(dbmsg.From.Value, contextConnection);
            //        }
            //    }
            //}
            //转发 message
            //int to >0
            try {
                if(Convert.ToInt32(to) > 0) {
                    XmppSeverConnection connection;
                    if(XmppConnectionDic.TryGetValue(to.ToString(), out connection)) {
                        connection.Send(msg);
                    }
                }
            } catch(Exception ignore) { }


            var insertSql=string.Format("INSERT INTO `message`( `content`, `from`, `to`, `subject`) VALUES ('{0}','{1}','{2}','{3}')", content, from, to, msg.Subject);
            var result =MysqlHelper.ExecuteNonQuery(insertSql);
            if(result==-1) {
                Console.WriteLine("exception  when insert  ->"+insertSql);
            }
            if(result==1) {
                Console.WriteLine("收到并插入成功一条消息 id->"+msg.Id);
            }
        }
    }
}
