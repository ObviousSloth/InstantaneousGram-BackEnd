using Microsoft.Azure.Cosmos;

namespace InstantaneousGram_LikeAndComment.Services
{
    public class CosmosDbService
    {
        private readonly Container _container;

        public CosmosDbService(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync<T>(T item)
        {
            await _container.CreateItemAsync(item);
        }

        public async Task<T> GetItemAsync<T>(string id, string userId)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, new PartitionKey(userId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        // Additional methods for updating and deleting items can be added here
    }
}
