using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDownloadAndUpload.Models;
namespace FileDownloadAndUpload.Core.Account
{
    public class AccountManage
    {
        Entities entis;
        static AccountManage instance;
        private  AccountManage()
        {
            entis = new Entities();
        }
        public static AccountManage GetInstance()
        {
            if (instance == null)
                instance = new AccountManage();
            return instance;
        }
        public bool CheckAccount(string uid,string psw)
        {
            User u = entis.User.Find(int.Parse(uid));
            if (u == null)
                return false;
            if (u.Password == null)
                return false;
            if (psw == null)
                return false;
            return string.Equals(u.Password.Trim(),psw.Trim(),StringComparison.OrdinalIgnoreCase);
        }
        public User GetUserInfo(string uid)
        {
            return entis.User.Find(int.Parse(uid));
        }


    }
}