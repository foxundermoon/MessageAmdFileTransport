using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace FileDownloadAndUpload.Core.Utils {
    public class MongoUtil {
        public static BsonDocument GetExceptionBsonDocument( Exception ex ) {
            return new BsonDocument { 
                        {"Message",ex.Message},
                        {"Source",ex.Source},
                        {"StackTrace",ex.StackTrace},
                        {"TargetSite",ex.TargetSite.ToString()},
                        {"Data",new BsonDocument(ex.Data)},
                        {"InerException", GetExceptionBsonDocument( ex.InnerException)},
                        {"HResult",ex.HResult},
                        {"HelpLink",ex.HelpLink},
                        {"Time",new BsonDateTime(DateTime.UtcNow)},
                    };
        }
    }
}
