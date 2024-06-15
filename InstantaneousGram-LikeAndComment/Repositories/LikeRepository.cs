using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using InstantaneousGram_LikesAndComments.Models;
using InstantaneousGram_LikesAndComments.Settings;
using Microsoft.Extensions.Options;

namespace InstantaneousGram_LikesAndComments.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public LikeRepository(CosmosClient cosmosClient, IOptions<CosmosDbSettings> cosmosDbSettings)
        {
            _cosmosClient = cosmosClient;
            var settings = cosmosDbSettings.Value;
            _container = _cosmosClient.GetContainer(settings.DatabaseName, settings.ContainerName);
        }

        public async Task<IEnumerable<Like>> GetLikesByUserIdAsync(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.userId = @userId").WithParameter("@userId", userId);
            var iterator = _container.GetItemQueryIterator<Like>(query);
            var results = new List<Like>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task<Like> GetLikeByUserAndPostAsync(string userId, string postId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.userId = @userId AND c.postId = @postId")
                .WithParameter("@userId", userId)
                .WithParameter("@postId", postId);
            var iterator = _container.GetItemQueryIterator<Like>(query);
            var result = await iterator.ReadNextAsync();
            return result.FirstOrDefault();
        }

        public async Task AddLikeAsync(Like like)
        {
            like.Id = Guid.NewGuid();  // Ensure unique ID
            await _container.CreateItemAsync(like, new PartitionKey(like.UserId));
        }

        public async Task DeleteLikeAsync(Guid likeId, string userId)
        {
            await _container.DeleteItemAsync<Like>(likeId.ToString(), new PartitionKey(userId));
        }

        public async Task DeleteLikesByUserIdAsync(string userId)
        {
            var likes = await GetLikesByUserIdAsync(userId);
            foreach (var like in likes)
            {
                await DeleteLikeAsync(like.Id, userId);
            }
        }
    }
}
