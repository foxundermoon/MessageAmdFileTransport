using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDownloadAndUpload.Core.Xmpp;
using FileDownloadAndUpload.Core.Xmpp.Handler;
using FoxundermoonLib.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace FileDownloadAndUpload
{
    public static class XmppConfig
    {
        /// <summary>
        /// 注册 Xmpp相关配置
        /// </summary>
        public static void Registor()
        {
            //启动xmpp服务
            XmppServer.GetInstance().StartUp();
            //启动诊断
            TraceManager.StartTrace();

            //添加消息处理器
            List<XmppHandler> handlers = XmppServer.GetInstance().XmppHandlers;
            handlers.Add(new MessageHandler());
            handlers.Add(new IQHandler());
            handlers.Add(new PresenceHandler());
            handlers.Add(new MyMessageHandler());
            //handlers.Add(new )
            Thread ui = new Thread(() =>
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run( new FileDownloadAndUpload.Core.Xmpp.UI.Manager());
                }
                );
            ui.IsBackground = false;

            ui.Start();


        }
    }
}