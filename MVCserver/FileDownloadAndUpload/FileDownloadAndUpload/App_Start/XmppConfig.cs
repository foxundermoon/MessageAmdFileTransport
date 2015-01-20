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
        }
    }
}