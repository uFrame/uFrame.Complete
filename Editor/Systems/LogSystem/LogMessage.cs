using System.Collections.Generic;
using uFrame.Editor.Database.Data;
using uFrame.Json;

namespace uFrame.Editor.LogSystem
{
    public class LogMessage : IMessage, IDataRecord
    {

        [JsonProperty]
        public string Identifier { get; set; }
        [JsonProperty]
        public MessageType MessageType { get; set; }
        [JsonProperty]
        public string Message { get; set; }
        
        public IRepository Repository { get; set; }
        public bool Changed { get; set; }
        public IEnumerable<string> ForeignKeys {
            get { yield break; }
        }
    }


}