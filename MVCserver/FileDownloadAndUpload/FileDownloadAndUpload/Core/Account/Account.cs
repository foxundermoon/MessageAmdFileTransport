﻿using System;
using System.Linq;
using System.Threading.Tasks;
namespace FileDownloadAndUpload.Core.Account {
    public class AccountManage {
        //Entities entis;
        static AccountManage instance;
        private AccountManage( ) {
            //entis = new Entities();
        }
        public static AccountManage GetInstance( ) {
            if(instance == null)
                instance = new AccountManage();
            return instance;
        }
        public bool CheckAccount( string name, string password ) {
            var sql = string.Format("SELECT * FROM `nj_用户表` WHERE `登录名`='{0}' and  `密码`='{1}'", name, password);
            return Core.Utils.MysqlHelper.ExecuteQueryHasRows(sql);
        }
    }
}