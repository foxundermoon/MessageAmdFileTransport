using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;

namespace MongodbTest {
    public partial class Form1 : Form {
        public Form1( ) {
            InitializeComponent();
        }

        private async void button1_Click( object sender, EventArgs e ) {

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetServer().GetDatabase("foo");
            var collection = database.GetCollection<BsonDocument>("bar");

            //await collection.InsertOneAsync(new BsonDocument("Name", "Jack"));


            //var list = await collection.Find(new BsonDocument("Name", "Jack"))
            //    .ToListAsync();


            //foreach(var document in list) {
            //    Console.WriteLine(document["Name"]);
            //}
        }

        private async void button2_Click( object sender, EventArgs e ) {
        //    var client = new MongoClient("mongodb://localhost:27017");
        //    var database = client.GetDatabase("jnsw");
        //    var collection = database.GetCollection<BsonDocument>("user");
        //    //for(var i=0; i<100; i++) {
        //    //    var u = new  User();
        //    //    u.Name=i+"";
        //    //    u.Password="123456";
        //    //     await collection.InsertOneAsync(u);
        //    //}

        //    User quser = new User { Password="123456" };
        //    var pasd = new BsonString("123456");
        //    var list = await collection.Find(qd =>qd["Password"] == pasd)
        //        .ToListAsync();
        //    Console.Write(list.ToJson());
        }

    }

    public partial class User {

        public int ID { get; set; }
        public string Name { get; set; }
        public byte[] RegTime { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Nullable<int> status { get; set; }
    }
}
