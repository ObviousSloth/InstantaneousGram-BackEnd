using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstantaneousGram_LikesAndComments.Models;
using InstantaneousGram_LikesAndComments.Repositories;

namespace InstantaneousGram_LikesAndComments.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;

        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<IEnumerable<Like>> GetLikesByUserIdAsync(string userId)
        {
            return await _likeRepository.GetLikesByUserIdAsync(userId);
        }

        public async Task<Like> GetLikeByUserAndPostAsync(string userId, string postId)
        {
            return await _likeRepository.GetLikeByUserAndPostAsync(userId, postId);
        }
        public async Task AddLikeAsync(Like like)
        {
            if (like == null)
            {
                Console.WriteLine("Received null like object.");
                return;
            }

            Console.WriteLine($"Attempting to add like for user {like.UserId} on post {like.PostId}");

            var existingLike = await _likeRepository.GetLikeByUserAndPostAsync(like.UserId, like.PostId);
            if (existingLike == null)
            {
                like.Id = Guid.NewGuid();  // Ensure unique ID
                Console.WriteLine($"Adding new like for user {like.UserId} on post {like.PostId} with ID {like.Id}");
                await _likeRepository.AddLikeAsync(like);
                Console.WriteLine($"Successfully added like for user {like.UserId} on post {like.PostId}");
            }
            else
            {
                Console.WriteLine($"Like already exists with ID {existingLike.Id} for user {like.UserId} on post {like.PostId}");
                // Optionally handle existing like scenario
            }
        }


        public async Task DeleteLikeAsync(Guid likeId, string userId)
        {
            await _likeRepository.DeleteLikeAsync(likeId, userId);
        }

        public async Task DeleteLikesByUserIdAsync(string userId)
        {
            var likes = await _likeRepository.GetLikesByUserIdAsync(userId);
            foreach (var like in likes)
            {
                await _likeRepository.DeleteLikeAsync(like.Id, userId);
            }
        }
    }
}
