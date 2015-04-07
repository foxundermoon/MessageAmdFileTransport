using System;
using System.Linq;
using FileDownloadAndUpload.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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
        public bool CheckAccount( string uid, string psw ) {
            var users = Xmpp.XmppServer.Instance.UsersCollection;
            //var query = new BsonDocument {
            //{ "password", new BsonString(psw) },
            //    { "name", new BsonString(uid.ToString()) },
            //};
            //var builder =  Builders<BsonDocument>.Filter;
            //var filter =builder.Eq("Name", uid) & builder.Eq("Password", psw);
            var query = (from u in users.AsQueryable<BsonDocument>()
                         where u["Name"] == uid && u["Password"] == psw
                         select u);
            //var f = query.Take(5);
            //foreach(var item in f) {
            //    Console.WriteLine(item);
            //}
            var r = query.Count();
            return query.Count()>=1;               
        }
        //public bool CheckAccount( string uid, string psw ) {
        //    var users = Xmpp.XmppServer.Instance.UsersCollection;
        //    var result = users.

        //    if(u == null)
        //        return false;
        //    if(u.Password == null)
        //        return false;
        //    if(psw == null)
        //        return false;
        //    return string.Equals(u.Password.Trim(), psw.Trim(), StringComparison.OrdinalIgnoreCase);
        //}

        //public async Task<bool> CheckAccountAsync( string uid, string psw ) {
            //var xmppServer = Xmpp.XmppServer.Instance;
            //var users = Xmpp.XmppServer.Instance.UsersCollection;
            ////var query = new BsonDocument {
            ////{ "password", new BsonString(psw) },
            ////    { "name", new BsonString(uid.ToString()) },
            ////};
            ////var builder =  Builders<BsonDocument>.Filter;
            ////var filter =builder.Eq("Name", uid) & builder.Eq("Password", psw);
            //var result = users.Find(filter).ToListAsync();
            //if(result!=null && (result.Count>=1))
            //    return true;
            //else
            //    return false;
        //}
        //public User GetUserInfo( string uid ) {
        //    return entis.User.Find(int.Parse(uid));
        //}


    }
}