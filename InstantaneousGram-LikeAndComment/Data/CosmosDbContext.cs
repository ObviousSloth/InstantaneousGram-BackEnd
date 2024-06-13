using InstantaneousGram_LikeAndComment.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using InstantaneousGram_LikeAndComment.Settings;

namespace InstantaneousGram_LikeAndComment.Data
{
    public class CosmosDbContext
    {
        private readonly Container _container;

        public CosmosDbContext(IOptions<CosmosDbSettings> settings)
        {
            var cosmosDbSettings = settings.Value;
            var client = new CosmosClient(cosmosDbSettings.ConnectionString);
            var database = client.GetDatabase(cosmosDbSettings.DatabaseName);
            _container = database.GetContainer(cosmosDbSettings.ContainerName);
        }

        public async Task AddLikeAsync(Like like)
        {
            // Ensure the partition key is set in the document
            if (string.IsNullOrEmpty(like.UserId))
            {
                throw new ArgumentNullException(nameof(like.UserId), "Partition key (UserId) cannot be null.");
            }
            await _container.CreateItemAsync(like, new PartitionKey(like.UserId));
        }

        public async Task AddCommentAsync(Comment comment)
        {
            // Ensure the partition key is set in the document
            if (string.IsNullOrEmpty(comment.UserId))
            {
                throw new ArgumentNullException(nameof(comment.UserId), "Partition key (UserId) cannot be null.");
            }
            await _container.CreateItemAsync(comment, new PartitionKey(comment.UserId));
        }

        // Additional methods for retrieving likes and comments can be added here.
    }

}
