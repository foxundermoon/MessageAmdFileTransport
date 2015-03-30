using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongodbTest {
    class Program {
        [STAThread]
        static void Main( string[] args ) {
            //System.Windows.Forms.Application.Run(new Form1());
            MongodbTest();
            //new Form1().Show();
        }

        private static void MongodbTest( ) {
            var client = new MongoClient("mongodb://10.80.5.222:27017");
            var database = client.GetDatabase("performTest_1000000");
            var collection = database.GetCollection<BsonDocument>("test1");
            try {
                Console.WriteLine("start....");
            var start = DateTime.UtcNow;
            for(var i=1; i<1000000; i++) {
                var dom = new BsonDocument { 
                {"name","test name"},
                {"number",i},
                {"datetime",DateTime.UtcNow}
                };
               
                collection.InsertOneAsync(dom);
                
            }
            var stop =DateTime.UtcNow;
            Console.WriteLine("complete");
            Console.WriteLine("start :"+start.ToFileTimeUtc());
            Console.WriteLine("stop:"+stop.ToFileTimeUtc());
            Console.WriteLine("---------------------\n间隔  :"+(stop-start));
            Console.WriteLine("second: stop->"+stop +" start->" + start);
            Console.Read();
            } catch(Exception e) {
                Console.WriteLine(e);
            }
        }

       
        //static void mongoDb( ) {
        //    try {

        //    var constr = "mongodb://localhost";
        //    var client = new MongoClient();
        //    var database = client.GetDatabase("test");
        //    var collection = database.GetCollection<Entity>("entites");
        //    for(var i=0; i<10; i++) {
        //        var entity = new Entity { Name="tname",Nid=i, };
        //        //await collection.InsertOneAsync(entity);
        //        Console.WriteLine("add"+i +" ;"+entity.ToJson());
        //    }

        //    Console.WriteLine("add complete");

        //    System.Threading.Thread.Sleep(1000);

        //    var result = await collection.FindAsync<Entity>(e=>e.Name =="tname");
        //        result.Start();
        //        result.Wait();

        //    var json=collection.ToJson();
        //    foreach(var item in result.Result.Current) {
        //        Console.WriteLine(item.ToJson());
        //    }
        //    var r = result.Result;

        //    var j = r.ToJson();
        //    //var query = new QueryDocument { };
        //    } catch(Exception ex) {
        //        Console.WriteLine(ex.ToString());
        //    }
        //    //var query = Query<Entity>()



        //}
    }


    public class Entity {
        public ObjectId Id { get; set; }
        public int Nid { get; set; }

        public string Name { get; set; }
    }
}
