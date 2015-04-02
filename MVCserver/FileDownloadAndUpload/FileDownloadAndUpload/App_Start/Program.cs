using System;
//using System.Collections.Generic;
using FileDownloadAndUpload.Core.Xmpp;

namespace FileDownloadAndUpload {
  public  class Program {
        [STAThread]
       public static void Main( string[] args ) {
            XmppLauncher.Launch();
            //XmppServer.GetInstance().StartUp();
        }
    }
}
