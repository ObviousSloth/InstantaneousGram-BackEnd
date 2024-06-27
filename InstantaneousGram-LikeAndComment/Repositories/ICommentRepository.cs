using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstantaneousGram_LikesAndComments.Models;

namespace InstantaneousGram_LikesAndComments.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string userId);
        Task<IEnumerable<Comment>> GetCommentsByUserAndPostAsync(string userId, string postId);
        Task AddCommentAsync(Comment comment);
        Task DeleteCommentAsync(Guid commentId, string userId);
        Task DeleteCommentsByUserIdAsync(string userId);
    }
}
