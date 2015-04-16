using System;
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
            var sql = string.Format("SELECT * FROM `user` WHERE `name`='{0}' and `password`=Password('{1}')", name, password);
            return Core.Utils.MysqlHelper.ExecuteQueryHasRows(sql);
        }
    }
}