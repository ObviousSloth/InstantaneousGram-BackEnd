using InstantaneousGram_LikeAndComment.Data;
using InstantaneousGram_LikeAndComment.Models;

namespace InstantaneousGram_LikeAndComment.Services
{

    public class LikeAndCommentService
    {
        private readonly CosmosDbContext _cosmosDbContext;

        public LikeAndCommentService(CosmosDbContext cosmosDbContext)
        {
            _cosmosDbContext = cosmosDbContext;
        }

        public async Task AddLikeAsync(Like like)
        {
            if (string.IsNullOrEmpty(like.Id))
            {
                like.Id = Guid.NewGuid().ToString();
            }
            await _cosmosDbContext.AddLikeAsync(like);
        }

        public async Task AddCommentAsync(Comment comment)
        {
            if (string.IsNullOrEmpty(comment.Id))
            {
                comment.Id = Guid.NewGuid().ToString();
            }
            await _cosmosDbContext.AddCommentAsync(comment);
        }

        // Additional methods for business logic related to likes and comments can be added here.
    }
}
