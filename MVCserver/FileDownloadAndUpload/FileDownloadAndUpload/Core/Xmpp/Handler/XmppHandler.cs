using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agsXMPP.Xml.Dom;
using agsXMPP;

namespace FileDownloadAndUpload.Core.Xmpp
{
    public abstract class XmppHandler
    {
        public Jid ServerJid
        {
            get
            {
                return new Jid("0@10.80.5.222");
            }
        }
        protected  Type HandlerType { get; set; }
        //public agsXMPP.XmppSeverConnection CurrentXmppServerConnection { get; set; }
        protected XmppHandler(Type t)
        {
            HandlerType = t;
        }
        public abstract void Process(agsXMPP.XmppSeverConnection contextConnection,Node node);

    }

}
