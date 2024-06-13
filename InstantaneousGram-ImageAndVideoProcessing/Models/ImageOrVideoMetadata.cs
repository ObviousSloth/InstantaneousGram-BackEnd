using Newtonsoft.Json;

namespace InstantaneousGram_ImageAndVideoProcessing.Models
{
    public class ImageOrVideoMetadata
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } // "image" or "video"

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
