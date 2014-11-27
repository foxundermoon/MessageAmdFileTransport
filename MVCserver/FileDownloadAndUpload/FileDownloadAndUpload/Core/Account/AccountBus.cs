using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileDownloadAndUpload.Core.Account
{
    public class AccountBus
    {
        public static bool CheckAccount(string uid ,string pwd)
        {
            AccountManage  m = AccountManage.GetInstance();
            return m.CheckAccount(uid, pwd);
        }
        
    }


}