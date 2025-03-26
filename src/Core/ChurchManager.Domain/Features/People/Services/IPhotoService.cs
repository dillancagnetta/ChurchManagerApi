using Codeboss.Results;
using Microsoft.AspNetCore.Http;

namespace ChurchManager.Domain.Features.People.Services
{
    public interface IPhotoService
    {
        Task<OperationResult<string>> AddPhotoAsync(string fileName, IFormFile file, CancellationToken ct = default);
        Task<OperationResult<string>> AddImageAsync(
            string fileName, IFormFile file, string folder = "", 
            int height = 500, 
            int width = 500,
            string aspectRatio = "16:9", CancellationToken ct = default);
        Task<OperationResult<string>> DeletePhotoAsync(string publicId);
    }
}
