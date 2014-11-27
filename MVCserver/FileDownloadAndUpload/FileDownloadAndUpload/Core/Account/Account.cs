using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDownloadAndUpload.Models;
namespace FileDownloadAndUpload.Core.Account
{
    public class AccountManage
    {
        MessageEntities1 entis;
        static AccountManage instance;
        private  AccountManage()
        {
            entis = new MessageEntities1();
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
            return u.Password == psw;
        }
        public User GetUserInfo(string uid)
        {
            return entis.User.Find(int.Parse(uid));
        }


    }
}