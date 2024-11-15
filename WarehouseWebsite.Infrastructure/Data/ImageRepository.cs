using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using WarehouseWebsite.Domain.Interfaces.Repositories;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class ImageRepository : IImageRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private string _containerName;

        public ImageRepository(BlobServiceClient client, string containerName)
        {
            _blobServiceClient = client;
            _containerName = containerName;
        }

        public async Task<Guid> UploadAsync(Stream stream, string contentType)
        {
            Guid imageId = Guid.NewGuid();
            var blobClient = BlobContainerClient.GetBlobClient(imageId.ToString());

            using (stream)
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = contentType
                });
            }
            return imageId;
        }

        public async Task DeleteAsync(Guid imageId)
        {
            var blobClient = BlobContainerClient.GetBlobClient(imageId.ToString());
            await blobClient.DeleteIfExistsAsync();
        }

        public string GetImageUri(Guid imageId)
        {
            var blobClient = BlobContainerClient.GetBlobClient(imageId.ToString());
            return blobClient.Uri.ToString();
        }

        private BlobContainerClient BlobContainerClient => _blobServiceClient.GetBlobContainerClient(_containerName);
    }
}
