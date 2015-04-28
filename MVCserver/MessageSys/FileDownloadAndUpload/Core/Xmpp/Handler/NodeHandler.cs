using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDownloadAndUpload.Core.Xmpp;
using agsXMPP;
using agsXMPP.Xml.Dom;
using FileDownloadAndUpload.Core.Utils;

namespace FileDownloadAndUpload.Core.Xmpp
{
    public partial class XmppServer
    {
        public void OnNode(XmppSeverConnection contextConnection, Node node)
        {
            //process all the node;
            //ig nore
            //if(node.GetType() == typeof(Element)) {
            //    Element e = node as Element;
            //    if(e.HasTag("base64")) {
            //        e.Value = EncryptUtil.DecryptBASE64ByGzip(e.Value);
            //    }
            //}
        }
    }
}