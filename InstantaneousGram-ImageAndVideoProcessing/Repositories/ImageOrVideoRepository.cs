using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantaneousGram_ImageAndVideoProcessing.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace InstantaneousGram_ImageAndVideoProcessing.Repositories
{
    public class ImageAndVideoRepository : IImageAndVideoRepository
    {
        private readonly Container _container;

        public ImageAndVideoRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<IEnumerable<ImageOrVideoMetadata>> GetAllMediaByUserIdAsync(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.userId = @userId")
                .WithParameter("@userId", userId);
            var iterator = _container.GetItemQueryIterator<ImageOrVideoMetadata>(query);
            var results = new List<ImageOrVideoMetadata>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<ImageOrVideoMetadata> GetMediaByIdAsync(string mediaId, string userId)
        {
            try
            {
                var response = await _container.ReadItemAsync<ImageOrVideoMetadata>(mediaId, new PartitionKey(userId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddMediaAsync(ImageOrVideoMetadata media)
        {
            await _container.CreateItemAsync(media, new PartitionKey(media.UserId));
        }

        public async Task UpdateMediaAsync(ImageOrVideoMetadata media)
        {
            await _container.UpsertItemAsync(media, new PartitionKey(media.UserId));
        }

        public async Task DeleteMediaAsync(string mediaId, string userId)
        {
            await _container.DeleteItemAsync<ImageOrVideoMetadata>(mediaId, new PartitionKey(userId));
        }
    }
}
