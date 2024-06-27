using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using InstantaneousGram_LikesAndComments.Models;
using InstantaneousGram_LikesAndComments.Settings;
using Microsoft.Extensions.Options;

namespace InstantaneousGram_LikesAndComments.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public CommentRepository(CosmosClient cosmosClient, IOptions<CosmosDbSettings> cosmosDbSettings)
        {
            _cosmosClient = cosmosClient;
            var settings = cosmosDbSettings.Value;
            _container = _cosmosClient.GetContainer(settings.DatabaseName, settings.ContainerName);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.userId = @userId").WithParameter("@userId", userId);
            var iterator = _container.GetItemQueryIterator<Comment>(query);
            var results = new List<Comment>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserAndPostAsync(string userId, string postId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.userId = @userId AND c.postId = @postId")
                .WithParameter("@userId", userId)
                .WithParameter("@postId", postId);
            var iterator = _container.GetItemQueryIterator<Comment>(query);
            var results = new List<Comment>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            comment.Id = Guid.NewGuid();  // Ensure unique ID
            await _container.CreateItemAsync(comment, new PartitionKey(comment.UserId));
        }

        public async Task DeleteCommentAsync(Guid commentId, string userId)
        {
            await _container.DeleteItemAsync<Comment>(commentId.ToString(), new PartitionKey(userId));
        }

        public async Task DeleteCommentsByUserIdAsync(string userId)
        {
            var comments = await GetCommentsByUserIdAsync(userId);
            foreach (var comment in comments)
            {
                await DeleteCommentAsync(comment.Id, userId);
            }
        }
    }
}
