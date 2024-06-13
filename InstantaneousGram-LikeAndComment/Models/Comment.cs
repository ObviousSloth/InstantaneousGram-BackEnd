using Newtonsoft.Json;

namespace InstantaneousGram_LikeAndComment.Models
{
    public class Comment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("postId")]
        public string PostId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
