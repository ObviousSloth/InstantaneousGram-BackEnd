using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstantaneousGram_LikesAndComments.Models;
using InstantaneousGram_LikesAndComments.Repositories;

namespace InstantaneousGram_LikesAndComments.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string userId)
        {
            return await _commentRepository.GetCommentsByUserIdAsync(userId);
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _commentRepository.AddCommentAsync(comment);
        }

        public async Task DeleteCommentAsync(Guid commentId, string userId)
        {
            await _commentRepository.DeleteCommentAsync(commentId, userId);
        }

        public async Task DeleteCommentsByUserIdAsync(string userId)
        {
            var comments = await _commentRepository.GetCommentsByUserIdAsync(userId);
            foreach (var comment in comments)
            {
                await _commentRepository.DeleteCommentAsync(comment.Id, userId);
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserAndPostAsync(string userId, string postId)
        {
            return await _commentRepository.GetCommentsByUserAndPostAsync(userId, postId);
        }
    }


}
