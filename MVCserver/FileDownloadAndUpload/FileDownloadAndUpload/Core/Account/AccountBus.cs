using System;
//using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileDownloadAndUpload.Core.Account
{
    public class AccountBus
    {
        public async static Task<bool> CheckAccountAsync(string uid ,string pwd)
        {
            AccountManage  m = AccountManage.GetInstance();
            return await m.CheckAccountAsync(uid, pwd);
        }
        
    }


}