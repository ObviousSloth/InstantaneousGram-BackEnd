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
            Console.WriteLine($"GetLikeByUserAndPostAsync called for user {userId} and post {postId}");
            var query = new QueryDefinition("SELECT * FROM c WHERE c.userId = @userId AND c.postId = @postId")
                .WithParameter("@userId", userId)
                .WithParameter("@postId", postId);
            var iterator = _container.GetItemQueryIterator<Like>(query);
            var result = await iterator.ReadNextAsync();
            var like = result.FirstOrDefault();

            if (like != null)
            {
                Console.WriteLine($"Found existing like with ID {like.Id} for user {userId} on post {postId}");
            }
            else
            {
                Console.WriteLine($"No existing like found for user {userId} on post {postId}");
            }

            return like;
        }

        public async Task AddLikeAsync(Like like)
        {
            Console.WriteLine($"AddLikeAsync called. Adding like with ID {like.Id} for user {like.UserId} on post {like.PostId}");
            try
            {
                await _container.CreateItemAsync(like, new PartitionKey(like.UserId));
                Console.WriteLine($"Like added with ID {like.Id} for user {like.UserId} on post {like.PostId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding like: {ex.Message}");
            }
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
