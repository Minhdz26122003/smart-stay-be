using System.IO;
using System.Threading.Tasks;

namespace SmartStay.Application.Interfaces;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(Stream fileStream, string fileName, string folder);
    Task<bool> DeleteImageAsync(string publicId);
    string GenerateSignedUrl(string publicId, int ttlMinutes = 15);
}
