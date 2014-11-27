using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agsXMPP.Xml.Dom;

namespace FileDownloadAndUpload.Core.Xmpp
{
    public abstract class XmppHandler
    {
        protected  Type HandlerType { get; set; }
        //public agsXMPP.XmppSeverConnection CurrentXmppServerConnection { get; set; }
        protected XmppHandler(Type t)
        {
            HandlerType = t;
        }
        public abstract void Process(agsXMPP.XmppSeverConnection contextConnection,Node node);

    }

}
