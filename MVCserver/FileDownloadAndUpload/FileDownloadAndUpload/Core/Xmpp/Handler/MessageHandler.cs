using System;
//using System.Collections.Generic;
using System.Linq;
using agsXMPP.protocol.client;
using agsXMPP.protocol;
using FileDownloadAndUpload;
using FileDownloadAndUpload.Core.Common;
using System.Diagnostics;
using FileDownloadAndUpload.Core.Xmpp;
using agsXMPP;
using FileDownloadAndUpload.Core.Utils;
using System.Data.Entity.Validation;
using MongoDB.Driver;
using MongoDB.Bson;
namespace FileDownloadAndUpload.Core.Xmpp {
    public partial class XmppServer {
        public void OnMessage( XmppSeverConnection contextConnection, Message message ) {
            //Console.WriteLine("从 "+message.From.User + "接受到消息 ,id:"+message.Id +" , length:"+ message.Body.Length);
            if(contextConnection.IsAuthentic) {
                processMessage(contextConnection, message);

            } else {
                contextConnection.Stop();
            }

        }
        private async  void processMessage( agsXMPP.XmppSeverConnection contextConnection, Message msg ) {
            //FileDownloadAndUpload.Models.Message dbmsg = entities.Message.Create();
            BsonDocument bdm = new BsonDocument();
            int from=-1;
            int to = -1;
            //Trace.Write(msg.ToString(),msg.GetType().ToString);
            //dbmsg.Status = 0;
            //dbmsg.Mid = msg.Id;
            if(!string.IsNullOrEmpty(msg.Language) && msg.Language.ToUpper().Contains("BASE64")) {
                bdm.Add("Content", EncryptUtil.DecryptBASE64ByGzip(msg.Body));
            } else {
                bdm.Add("Content", msg.Body);
                //dbmsg.Content = msg.Body;
            }
            if(msg.To != null) {
                int toUid = 0;
                try {
                    toUid = int.Parse(msg.To.User);
                    bdm.Add("To", toUid+"");
                    to = toUid;
                } catch(FormatException fe) {
                    //Trace.Write(fe.Message, fe.GetType().FullName);
                    //throw fe;
                }

                if(msg.From != null) {
                    bdm.Add("Resource", msg.From.Resource);
                    //dbmsg.Resource = msg.From.Resource;
                    int fromUid = -1;
                    try {
                        fromUid = int.Parse(msg.From.User);

                        bdm.Add("From", fromUid+"");
                        from= fromUid;
                        //dbmsg.From = fromUid;

                    } catch(FormatException fe) {
                        //Trace.Write("format  the user id of From catch Exception \n:" + fe.Message, fe.GetType().FullName);
                    }
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
                //int to =
                if(to > 0) {
                    XmppSeverConnection connection;
                    if(XmppConnectionDic.TryGetValue(to.ToString(), out connection)) {
                        connection.Send(msg);
                    }
                }

            }
            bdm.Add("ReceiveTime",new BsonDateTime(DateTime.UtcNow));
            SaveDocument(bdm);
            //await  MessageCollction.InsertOneAsync(bdm);
            //}
            //catch(Exception ex) {
            //    Console.WriteLine("mongodb exception->"+ex.Message +   ex.Source +ex.StackTrace);
            //    InitMongoClient();
            //    ExceptionCollection.InsertOneAsync(MongoUtil.GetExceptionBsonDocument(ex));
            //}
            //entities.Message.Add(dbmsg);
            //try {
            //    entities.SaveChanges();
            //    Console.WriteLine(dbmsg.Mid + "保存成功");
            //} catch(DbEntityValidationException ex) {
            //    //Trace.Write("save message to database catch exception :" + ex.Message, "messageEntities.SaveChanges()");
            //    foreach(var i in ex.EntityValidationErrors) {
            //        foreach(var j in i.ValidationErrors) {
            //            Console.WriteLine(j.PropertyName + ":" + j.ErrorMessage);
            //        }
            //    } 
            //}

        }
    }
}
