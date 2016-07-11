using uFrame.Json;

namespace uFrame.Editor.Database.Data
{
    public class ExportedRecord
    {
        [JsonProperty]
        public string Identifier { get; set; }
        [JsonProperty]
        public string Data { get; set; }
    }
}