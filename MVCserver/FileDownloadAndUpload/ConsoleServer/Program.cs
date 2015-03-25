using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileDownloadAndUpload.Core.Xmpp;

namespace ConsoleServer {
    class Program {
        static void Main( string[] args ) {
            XmppServer.GetInstance().StartUp();
        }
    }
}
