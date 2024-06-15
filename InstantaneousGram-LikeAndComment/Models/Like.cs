using Newtonsoft.Json;
using System;

namespace InstantaneousGram_LikesAndComments.Models
{
    public class Like
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; } = string.Empty;

        [JsonProperty("postId")]
        public string PostId { get; set; } = string.Empty;

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = "like";
    }
}
