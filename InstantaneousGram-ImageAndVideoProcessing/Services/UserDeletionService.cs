using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InstantaneousGram_ImageAndVideoProcessing.Services
{
    public class UserDeletionService
    {
        private readonly IImageAndVideoService _imageAndVideoService;
        private readonly ILogger<UserDeletionService> _logger;

        public UserDeletionService(IImageAndVideoService imageAndVideoService, ILogger<UserDeletionService> logger)
        {
            _imageAndVideoService = imageAndVideoService;
            _logger = logger;
        }

        public async Task HandleUserDeletedAsync(string userId)
        {
            _logger.LogInformation($"Handling deletion of all media for user ID: {userId}");
            await _imageAndVideoService.DeleteMediaByUserIdAsync(userId);
            _logger.LogInformation($"Completed deletion of all media for user ID: {userId}");
        }
    }
}
