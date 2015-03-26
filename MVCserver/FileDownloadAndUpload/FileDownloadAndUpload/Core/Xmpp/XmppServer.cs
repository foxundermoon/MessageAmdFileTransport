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

namespace FileDownloadAndUpload.Core.Xmpp
{
    public partial  class XmppServer
    {
        Models.Entities entities = Core.Common.MessageEntityFictory.ModelsEntities;
        // Thread signal.
        private ManualResetEvent allDone = new ManualResetEvent(false);
        private Socket listener;
        private bool m_Listening;
        public MessageHandle MsgHandle { get; private set; }
        static XmppServer instance;
        public Dictionary<int, XmppSeverConnection> XmppConnectionDic { get; private set; }
        //public event EventHandler<int> ConnectionEncrease;
        //public event EventHandler ConnectionDecrease;
        public static Jid ServerJid;

        private XmppServer()
        {
            XmppConnectionDic = new Dictionary<int, XmppSeverConnection>();
            ServerJid = new agsXMPP.Jid("0", agsXMPP.XmppConfig.ServerIP, "server");
        }
        public static XmppServer GetInstance()
        {
            if (instance == null)
                instance = new XmppServer();
            return instance;
        }
        public void StartUp()
        {
            ThreadStart myThreadDelegate = new ThreadStart(Listen);
            Thread myThread = new Thread(myThreadDelegate);
            myThread.Start();
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
        private void Listen()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 5222);

            // Create a TCP/IP socket.
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                m_Listening = true;

                while (m_Listening)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    //Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), null);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }

        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();
            // Get the socket that handles the client request.
            Socket newSock = listener.EndAccept(ar);

            XmppSeverConnection con = new XmppSeverConnection(newSock, this);
            con.OnNode+= new NodeHandler(OnNode);
            con.OnIq += new IqHandler(OnIQ);
            con.OnMessage += new MessageHandler(OnMessage);
            con.OnPresence += new PresenceHandler(OnPresence);
            /// you can  register other handler  here
            //listener.BeginReceive(buffer, 0, BUFFERSIZE, 0, new AsyncCallback(ReadCallback), null);
        }


        private void stop()
        {
            m_Listening = false;
            allDone.Set();
            //allDone.Reset();
        }

        public void Broadcast(string strMsg, Xmpp.Type type)
        {
            if (type == Xmpp.Type.Message)
            {
                Message msg = new Message();
                msg.From = new Jid("0@10.80.5.222/Server");
                foreach (var con in XmppConnectionDic)
                {
                    Jid to = new Jid(con.Key + "@10.80.5.222");
                    msg.To = to;
                    con.Value.Send(msg);
                }

            }
            if (type == Xmpp.Type.Notification)
            {

                IQ notificationIQ = new IQ();
                Element notify = new Element();
                notify.Namespace = "androidpn:iq:notification";
                notify.TagName = "notification";
                notify.AddChild(new Element("id", Guid.NewGuid().ToString()));
                notificationIQ.AddChild(notify);
                notificationIQ.From = new Jid("0@10.80.5.222/Server");

                foreach (var con in XmppConnectionDic)
                {
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
        public void Broadcast(string strMsg)
        {
            Message msg = new Message();
            msg.From = new Jid("0@10.80.5.222/Server");
            msg.Body = strMsg;
            foreach (var con in XmppConnectionDic)
            {
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
        public void Unicast(int id, string strMsg)
        {
            if (XmppConnectionDic.ContainsKey(id))
            {
                XmppSeverConnection value;
                if (XmppConnectionDic.TryGetValue(id, out value))
                {
                    Message msg = new Message();
                    msg.To = new Jid(id + "@10.80.5.222");
                    msg.Body = strMsg;
                    value.Send(msg);
                }

            }
        }

        internal void Broadcast(agsXMPP.protocol.Base.Stanza reply)
        {
            foreach (var con in XmppConnectionDic)
            {
                Jid to = new Jid(con.Key + "@10.80.5.222");
                reply.To = to;
                con.Value.Send(reply);
            }
        }
    }
}