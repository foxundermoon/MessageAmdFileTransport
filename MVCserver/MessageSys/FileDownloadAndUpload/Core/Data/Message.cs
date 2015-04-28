using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace FileDownloadAndUpload.Core.Data
{
   public class Message
    {
       public string Id { get; set; }
       public DataTable DataTable { get; set;}

       public IDictionary<string, string> Propertites { get; set; }

       public Command Command { get; set; }
       public void SetJsonCommand(string command)
       {
           Command = JsonConvert.DeserializeObject<Command>(command);
       }

       public string GetJsonCommand()
       {
           return JsonConvert.SerializeObject(Command);
       }
       public JObject ToJsonObject()
       {
           JObject jmessage = new JObject();
           jmessage.Add(DicKeys.id, Id);
           foreach(var item in Propertites){
               jmessage.Add(item.Key,item.Value);
           }
           if()
           jmessage.Add(DicKeys.dataTable, JsonConvert.SerializeObject(DataTable,));

           return jmessage;
       }
       public string ToJson()
       {
           return ToJsonObject().ToString();
       }

       public void AddProperty(string key,string value)
       {
           if (Propertites.ContainsKey(key))
           {
               Propertites.Remove(key);
           }
           Propertites.Add(key, value);
       }
        
    }
}
