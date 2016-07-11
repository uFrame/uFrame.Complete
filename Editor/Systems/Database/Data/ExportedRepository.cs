using System.Collections.Generic;
using uFrame.Json;

namespace uFrame.Editor.Database.Data
{
    public class ExportedRepository
    {
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]
        public List<ExportedRecord> Records { get; set; }
    }
}