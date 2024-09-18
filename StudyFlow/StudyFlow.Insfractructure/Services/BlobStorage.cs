﻿using StudyFlow.Insfractructure.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Threading.Tasks;
using System.Runtime.ConstrainedExecution;
using Microsoft.Extensions.DependencyInjection;

namespace StudyFlow.Insfractructure.Services
{
    public class BlobStorage : IBlobStorage
    {
        private string _connectionString;
        private readonly string _containerString;

        public BlobStorage(string containerString)
        {
            _containerString = containerString;
        }

        public void ConfigureBlobStorage(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var keyVaultService = serviceProvider.GetService<IKeyVaultService>();

            if (keyVaultService == null)
            {
                throw new InvalidOperationException("IKeyVaultService is not registered in the service collection.");
            }

            _connectionString = keyVaultService.GetSecretAsync("BlobStorageConnectionString").Result;
        }

        public async Task<bool> DeleteAsync(string blobName)
        {
            try
            {
                BlobContainerClient containerClient = new BlobContainerClient(_connectionString, _containerString);
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                BlobProperties properties = await blobClient.GetPropertiesAsync();

                if (properties != null)
                {
                    await blobClient.DeleteAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> DownloadAsync(string blobName)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerString);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            try
            {
                var downloadResponse = await blobClient.DownloadAsync();

                using (var memoryStream = new MemoryStream())
                {
                    await downloadResponse.Value.Content.CopyToAsync(memoryStream);
                    byte[] blobBytes = memoryStream.ToArray();
                    string base64String = Convert.ToBase64String(blobBytes);
                    return base64String;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UploadAsync(string base64String, string blobName)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerString);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            try
            {
                byte[] blobBytes = Convert.FromBase64String(base64String);

                using (var memoryStream = new MemoryStream(blobBytes))
                {
                    await blobClient.UploadAsync(memoryStream, overwrite: true);
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}