using InstantaneousGram_ImageAndVideoProcessing.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace InstantaneousGram_ImageAndVideoProcessing.Services
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

        public Container Container => _container;

        public async Task AddItemAsync<T>(T item)
        {
            await _container.CreateItemAsync(item);
        }

        public async Task<T> GetItemAsync<T>(string id, string partitionKey)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task DeleteItemAsync(string id, string partitionKey)
        {
            await _container.DeleteItemAsync<ImageOrVideoMetadata>(id, new PartitionKey(partitionKey));
        }
    }
}
