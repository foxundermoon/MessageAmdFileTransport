using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileDownloadAndUpload.Core.Xmpp;

namespace FileDownloadAndUpload {
  public  class Program {
        [STAThread]
       public static void Main( string[] args ) {
            XmppConfig.Registor();
            //XmppServer.GetInstance().StartUp();
        }
    }
}
