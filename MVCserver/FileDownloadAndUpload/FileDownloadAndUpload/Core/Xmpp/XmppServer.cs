using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;
using MongoDB.Driver;
using System.Configuration;
using FileDownloadAndUpload.Core.Config;
using MongoDB.Bson;
using FileDownloadAndUpload.Core.Utils;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
namespace FileDownloadAndUpload.Core.Xmpp {
    public partial class XmppServer {
        static object _lock = new object();
        Models.Entities entities = Core.Common.MessageEntityFictory.ModelsEntities;
        // Thread signal.
        private ManualResetEvent allDone = new ManualResetEvent(false);
        private Socket listener;
        private bool m_Listening;
        static XmppServer instance;
        public Dictionary<int, XmppSeverConnection> XmppConnectionDic { get; private set; }
        //public event EventHandler<int> ConnectionEncrease;
        //public event EventHandler ConnectionDecrease;
        public static Jid ServerJid;
        public MongoDB.Driver.IMongoClient Mongo;
        public IMongoDatabase MongoDatabase;
        public IMongoCollection<BsonDocument> UsersCollection;
        public IMongoCollection<BsonDocument> MessageCollction;
        public IMongoCollection<BsonDocument> ExceptionCollection;
        public Config.ServerConfig Config;
        ActionBlock<BsonDocument> messageActionBlock;
        public static XmppServer Instance {
            get {
                lock(_lock) {
                    if(instance==null) {
                        instance = new XmppServer();
                    }
                    return instance;
                }
            }
        }
        public void SaveMessage( BsonDocument document ) {
            messageActionBlock.Post(document);
        }
        private XmppServer( ) {
            initConfig();
            XmppConnectionDic = new Dictionary<int, XmppSeverConnection>();
            ServerJid = new agsXMPP.Jid(Config.ServerUid.ToString(), Config.ServerIp, Config.ServerResource);
           
        }

        private void initConfig( ) {
            Config = new ServerConfig();
            Config.FileCollection = ConfigurationManager.AppSettings["FileCollection"].ToString();
            Config.FileServer = ConfigurationManager.AppSettings["FileServer"].ToString();
            Config.FileServerPort =int.Parse(ConfigurationManager.AppSettings["FileServerPort"].ToString());
            Config.LogLevel=int.Parse(ConfigurationManager.AppSettings["FileServerPort"].ToString());
            Config.MessageCollection=  ConfigurationManager.AppSettings["MessageCollection"].ToString();
            Config.MongoDatabase=  ConfigurationManager.AppSettings["MongoDatabase"].ToString();
            Config.MongoServer=  ConfigurationManager.AppSettings["MongoServer"].ToString();
            Config.UserCollection= ConfigurationManager.AppSettings["UserCollection"].ToString();
            Config.XmppPort =int.Parse(ConfigurationManager.AppSettings["XmppServerPort"].ToString());
            Config.ServerResource = ConfigurationManager.AppSettings["ServerResource"].ToString();
            Config.ServerIp=  ConfigurationManager.AppSettings["ServerIp"].ToString();
           Config.ServerUid=int.Parse(ConfigurationManager.AppSettings["ServerUid"].ToString());
            //ConfigurationManager.AppSettings["ServerResource"].ToString();
            //ConfigurationManager.AppSettings["ServerResource"].ToString();
            //ConfigurationManager.AppSettings["ServerResource"].ToString();
            //ConfigurationManager.AppSettings["ServerResource"].ToString();
            //ConfigurationManager.AppSettings["ServerResource"].ToString();
            //ConfigurationManager.AppSettings["ServerResource"].ToString();
            //ConfigurationManager.AppSettings["ServerResource"].ToString();

        }
        public static XmppServer GetInstance( ) {
            return Instance;
        }
        public void InitMongoClient( ) {
            var mongoSetting = new MongoClientSettings();
            mongoSetting.ConnectionMode = ConnectionMode.Automatic;
            mongoSetting.MaxConnectionPoolSize= Convert.ToInt32(ConfigurationManager.AppSettings["MongoMaxConnectionPoolSize"].ToString());
            mongoSetting.MinConnectionPoolSize =Convert.ToInt32(ConfigurationManager.AppSettings["MongoMinConnectionPoolSize"].ToString());
            mongoSetting.Server = new MongoServerAddress(ConfigurationManager.AppSettings["MongoServer"], Convert.ToInt32(ConfigurationManager.AppSettings["MongoServerPort"]));
            mongoSetting.SocketTimeout = TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["MongoSocketTimeout"]));
            Mongo = new MongoClient(mongoSetting);
            MongoDatabase = Mongo.GetDatabase(Config.MongoDatabase);
            UsersCollection = MongoDatabase.GetCollection<BsonDocument>(Config.UserCollection);
            MessageCollction = MongoDatabase.GetCollection<BsonDocument>(Config.MessageCollection);
            ExceptionCollection = MongoDatabase.GetCollection<BsonDocument>(ConfigurationManager.AppSettings["ExceptionCollection"]);
        }
        public void StartUp( ) {
            try {
                InitMongoClient();
                initDatafolw();
                ThreadStart myThreadDelegate = new ThreadStart(Listen);
                Thread myThread = new Thread(myThreadDelegate);
                Console.WriteLine("开始监听 xmpp服务");
                myThread.Start();
            } catch(Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

        private void initDatafolw( ) {
            messageActionBlock = new ActionBlock<BsonDocument>(document => {
                MessageCollction.InsertOneAsync(document);
            });
        }

        //废弃
        //private void StartUpInNewThread()
        //{
        //    ThreadStart myThreadDelegate = new ThreadStart(Listen);
        //    Thread myThread = new Thread(myThreadDelegate);
        //    myThread.Start();

        //}

        //private void watch()
        //{
        //    while (true)
        //    {
        //        int count = XmppConnectionDic.Count;
        //        Thread.Sleep(1000);
        //        if (count == XmppConnectionDic.Count)
        //            return;
        //        else if (count < XmppConnectionDic.Count)
        //            ConnectionDecrease();
        //        else if (count > XmppConnectionDic.Count)
        //            ConnectionEncrease();
        //    }
        //}
        private void Listen( ) {
            try {
                int port =Config.XmppPort;
                if(port<1024)
                    port = 5222;
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

                // Create a TCP/IP socket.
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


                // Bind the socket to the local endpoint and listen for incoming connections.
                try {
                    listener.Bind(localEndPoint);
                    int loglevel = Config.LogLevel;
                    if(loglevel<0 || loglevel>10)
                        loglevel=10;
                    listener.Listen(loglevel);

                    m_Listening = true;

                    while(m_Listening) {
                        // Set the event to nonsignaled state.
                        allDone.Reset();

                        // Start an asynchronous socket to listen for connections.
                        //Console.WriteLine("Waiting for a connection...");
                        listener.BeginAccept(new AsyncCallback(AcceptCallback), null);

                        // Wait until a connection is made before continuing.
                        allDone.WaitOne();
                    }

                } catch(Exception ex) {
                    Console.WriteLine(ex.ToString());
                    ExceptionCollection.InsertOneAsync(MongoUtil.GetExceptionBsonDocument(ex));
                }

            } catch(Exception e) {
                Console.WriteLine(e.ToString());
                ExceptionCollection.InsertOneAsync(MongoUtil.GetExceptionBsonDocument(e));
            }
        }

        private void AcceptCallback( IAsyncResult ar ) {
            // Signal the main thread to continue.
            allDone.Set();
            // Get the socket that handles the client request.
            Socket newSock = listener.EndAccept(ar);

            Console.WriteLine("从 "+newSock.RemoteEndPoint.ToString() +"建立了一条tcp连接");

            XmppSeverConnection con = new XmppSeverConnection(newSock, this);
            con.OnNode+= new NodeHandler(OnNode);
            con.OnIq += new IqHandler(OnIQ);
            con.OnMessage += new MessageHandler(OnMessage);
            con.OnPresence += new PresenceHandler(OnPresence);
            /// you can  register other handler  here
            //listener.BeginReceive(buffer, 0, BUFFERSIZE, 0, new AsyncCallback(ReadCallback), null);
        }


        private void stop( ) {
            m_Listening = false;
            allDone.Set();
            //allDone.Reset();
        }

        public void Broadcast( string strMsg, Xmpp.Type type ) {
            if(type == Xmpp.Type.Message) {
                Message msg = new Message();
                msg.From = new Jid("0@10.80.5.222/Server");
                foreach(var con in XmppConnectionDic) {
                    Jid to = new Jid(con.Key + "@10.80.5.222");
                    msg.To = to;
                    con.Value.Send(msg);
                }

            }
            if(type == Xmpp.Type.Notification) {

                IQ notificationIQ = new IQ();
                Element notify = new Element();
                notify.Namespace = "androidpn:iq:notification";
                notify.TagName = "notification";
                notify.AddChild(new Element("id", Guid.NewGuid().ToString()));
                notificationIQ.AddChild(notify);
                notificationIQ.From = new Jid("0@10.80.5.222/Server");

                foreach(var con in XmppConnectionDic) {
                    Jid to = new Jid(con.Key + "@10.80.5.222");
                    notificationIQ.To = to;
                    con.Value.Send(notificationIQ);
                }


                //<iq xmlns="jabber:client" from="0@10.80.5.222/Server">
                //<notification xmlns="androidpn:iq:notification">
                //<id>3fada5a4-3f2e-4652-bf8c-01bb4df0debb</id>
                //</notification>
                //</iq>

            }
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="strMsg">广播的消息</param>
        public void Broadcast( string strMsg ) {
            Message msg = new Message();
            msg.From = new Jid("0@10.80.5.222/Server");
            msg.Body = strMsg;
            foreach(var con in XmppConnectionDic) {
                Jid to = new Jid(con.Key + "@10.80.5.222");
                msg.To = to;
                con.Value.Send(msg);
            }
        }
        /// <summary>
        /// 单播
        /// </summary>
        /// <param name="id">客户端  或者用户id</param>
        /// <param name="strMsg">发送的数据</param>
        public void Unicast( int id, string strMsg ) {
            if(XmppConnectionDic.ContainsKey(id)) {
                XmppSeverConnection value;
                if(XmppConnectionDic.TryGetValue(id, out value)) {
                    Message msg = new Message();
                    msg.To = new Jid(id + "@10.80.5.222");
                    msg.Body = strMsg;
                    value.Send(msg);
                }

            }
        }

        internal void Broadcast( agsXMPP.protocol.Base.Stanza reply ) {
            foreach(var con in XmppConnectionDic) {
                Jid to = new Jid(con.Key + "@10.80.5.222");
                reply.To = to;
                con.Value.Send(reply);
            }
        }

    }
}