using InstantaneousGram_LikeAndComment.Models;
using InstantaneousGram_LikeAndComment.Services;
using System.Threading.Tasks;

namespace InstantaneousGram_LikeAndComment.Data
{
public class CommentRepository
{
    private readonly CosmosDbService _cosmosDbService;

    public CommentRepository(CosmosDbService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }

    public async Task AddCommentAsync(Comment comment)
    {
        await _cosmosDbService.AddItemAsync(comment);
    }

    public async Task<Comment> GetCommentAsync(string id, string userId)
    {
        return await _cosmosDbService.GetItemAsync<Comment>(id, userId);
    }

    // Additional methods for updating and deleting comments can be added here
}
}
