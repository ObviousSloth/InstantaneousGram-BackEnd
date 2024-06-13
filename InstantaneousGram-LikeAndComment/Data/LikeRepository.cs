using InstantaneousGram_LikeAndComment.Models;
using InstantaneousGram_LikeAndComment.Services;
using System.Threading.Tasks;

namespace InstantaneousGram_LikeAndComment.Data
{
    public class LikeRepository
    {
        private readonly CosmosDbService _cosmosDbService;

        public LikeRepository(CosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        public async Task AddLikeAsync(Like like)
        {
            await _cosmosDbService.AddItemAsync(like);
        }

        public async Task<Like> GetLikeAsync(string id, string userId)
        {
            return await _cosmosDbService.GetItemAsync<Like>(id, userId);
        }

        // Additional methods for updating and deleting likes can be added here
    }
}
