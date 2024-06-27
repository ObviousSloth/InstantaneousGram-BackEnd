using System.Collections.Generic;
using System.Threading.Tasks;
using InstantaneousGram_LikesAndComments.Models;

namespace InstantaneousGram_LikesAndComments.Repositories
{
    public interface ILikeRepository
    {
        Task<IEnumerable<Like>> GetLikesByUserIdAsync(string userId);
        Task<Like> GetLikeByUserAndPostAsync(string userId, string postId);
        Task AddLikeAsync(Like like);
        Task DeleteLikeAsync(Guid likeId, string userId);
        Task DeleteLikesByUserIdAsync(string userId);
    }
}
