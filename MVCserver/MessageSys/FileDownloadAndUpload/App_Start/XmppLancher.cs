using System;
using System.Linq;
using FileDownloadAndUpload.Core.Xmpp;
using FileDownloadAndUpload.Core.Xmpp.Handler;
//using FoxundermoonLib.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace FileDownloadAndUpload
{
    public static class XmppLauncher
    {
        /// <summary>
        /// 注册 Xmpp相关配置
        /// </summary>
        public static void Launch()
        {
            Console.WriteLine("Starting ......");
            //启动xmpp服务
            XmppServer.GetInstance().StartUp();
            Console.WriteLine("Started");
            //启动诊断
            //TraceManager.StartTrace();
        }
    }
}