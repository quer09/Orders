using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Orders.BackEnd.Helpers
{
    public class FileStorage : IFileStorage
    {
        private readonly string _connectionString;

        public FileStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureStorage")!;
        }

        public async Task RemoveFileAsync(string path, string containerName)
        {
            var client = new BlobContainerClient(_connectionString, containerName);
            await client.CreateAsync();
            var fileName = Path.GetFileName(path);
            var blo = client.GetBlobClient(fileName);
            await blo.DeleteIfExistsAsync();
        }

        public async Task<string> SaveFileAsync(byte[] content, string extension, string containerName)
        {
            var client = new BlobContainerClient(_connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);

            using (var ms = new MemoryStream())
            {
                await blob.UploadAsync(ms);
            }

            return blob.Uri.ToString();
        }
    }
}