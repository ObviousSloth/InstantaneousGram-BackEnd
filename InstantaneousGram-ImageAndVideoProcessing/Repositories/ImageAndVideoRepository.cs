using InstantaneousGram_ImageAndVideoProcessing.Models;
using InstantaneousGram_ImageAndVideoProcessing.Services;
using Microsoft.Azure.Cosmos;

namespace InstantaneousGram_ImageAndVideoProcessing.Repositories
{
    public class ImageAndVideoRepository
    {
        private readonly CosmosDbService _cosmosDbService;

        public ImageAndVideoRepository(CosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        public async Task AddMetadataAsync(ImageOrVideoMetadata metadata)
        {
            await _cosmosDbService.AddItemAsync(metadata);
        }

        public async Task<ImageOrVideoMetadata> GetMetadataAsync(string id, string userId)
        {
            return await _cosmosDbService.GetItemAsync<ImageOrVideoMetadata>(id, userId);
        }

        public async Task<IEnumerable<ImageOrVideoMetadata>> GetAllByUserAsync(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.userId = @userId")
                        .WithParameter("@userId", userId);

            var iterator = _cosmosDbService.Container.GetItemQueryIterator<ImageOrVideoMetadata>(query);

            var results = new List<ImageOrVideoMetadata>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        // Additional methods for updating and deleting metadata can be added here
    }
}
