using uFrame.Json;

namespace uFrame.Editor.Koinonia.Data
{
    public class UFramePackagePreviewDescriptor
    {
        [JsonProperty]
        public string Id { get; set; }
        [JsonProperty]
        public string Title { get; set; }
        [JsonProperty]
        public string Description { get; set; }
        [JsonProperty]
        public UFramePackageManagementType ManagementType { get; set; }
        [JsonProperty]
        public string ProjectPreviewIconUrl { get; set; }
        [JsonProperty]
        public string LatestPublicVersionTag { get; set; }

    }
}
