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
using MongoDB.Driver.Linq;

namespace MongodbTest {
    class Program {
        [STAThread]
        static void Main( string[] args ) {
            //System.Windows.Forms.Application.Run(new Form1());
            if(args!=null && args.Length>0) {
                MongodbTest(args[0]);
            }
            MongoLinqTest();

            //new Form1().Show();
        }

        private static void MongoLinqTest( ) {
            var seetings = new MongoClientSettings();
            var _lock = new object();
            seetings.MaxConnectionPoolSize=1000;
            seetings.Server = new MongoDB.Driver.MongoServerAddress("10.80.5.251", 27017);
            var client = new MongoClient(seetings);
            var database = client.GetDatabase("performTest_1000000");
            var collection = database.GetCollection<BsonDocument>("test1");


        }

        private static void MongodbTest(string arg ) {
            var args = arg.Split(':');
            var from = Convert.ToInt32(args[0]);
            var to = Convert.ToInt32(args[1]);
            var seetings = new MongoClientSettings();
            var _lock = new object();
            seetings.MaxConnectionPoolSize=1000;
            seetings.Server = new MongoDB.Driver.MongoServerAddress("10.80.5.251", 27017);
            var client = new MongoClient(seetings);
            var database = client.GetDatabase("performTest_1000000");
            var collection = database.GetCollection<BsonDocument>("test1");
            var start = DateTime.UtcNow;
            int count =0;
            Console.WriteLine("start....");
            //Parallel.For(0, 1000000,  i => {
            //    var dom = new BsonDocument { 
            //{"name","test name"},
            //{"number",i},
            //{"datetime",DateTime.UtcNow}
            //};
            //    //if(MongoWaitQueue.)
            //    //lock(_lock) {
            //         collection.InsertOneAsync(dom);
            //    //}
            //    count++;
            //    if(count==100) {
            //        count=0;
            //        Console.WriteLine("inserted "+i);
            //    }
            //});
            try {

                for(var i=from; i<to; i++) {
                    var dom = new BsonDocument { 
                {"name","test name"},
                {"number",i},
                {"datetime",DateTime.UtcNow}
                };

                    collection.InsertOneAsync(dom);
                    count++;
                    if(count==100) {
                        count=0;
                        Console.WriteLine("inserted "+i);
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            var stop =DateTime.UtcNow;
            Console.WriteLine("complete");
            Console.WriteLine("start :"+start.ToFileTimeUtc());
            Console.WriteLine("stop:"+stop.ToFileTimeUtc());
            Console.WriteLine("---------------------\n间隔  :"+(stop-start));
            Console.WriteLine("second: stop->"+stop +" start->" + start);
            Console.Read();
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


    public class Entity {
        public ObjectId Id { get; set; }
        public int Nid { get; set; }

        public string Name { get; set; }
    }
}
