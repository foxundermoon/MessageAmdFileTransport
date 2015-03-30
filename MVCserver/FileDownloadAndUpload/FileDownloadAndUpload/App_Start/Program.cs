using System;
//using System.Collections.Generic;
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
