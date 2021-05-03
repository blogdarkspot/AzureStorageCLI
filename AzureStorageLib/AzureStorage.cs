using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureStorageLib
{
    public class AzureStorage
    {
        private string _connString;
        private BlobServiceClient _blobServiceClient;
        private BlobContainerClient _blobContainerClient;

        public string connString
        {
            get { return _connString; }
            set { _connString = value; }
        }

        public void Init()
        {
            _blobServiceClient = new BlobServiceClient(_connString);
        }

        public void Stop()
        {
            _blobContainerClient = null;
            _blobServiceClient = null;
        }

        public async Task<BlobContainerClient> CreateContainerAsync(string containerName)
        {
            return await _blobServiceClient.CreateBlobContainerAsync(containerName);
        }

        public List<string> GetContainersName()
        {
            var containers = _blobServiceClient.GetBlobContainers();
            var names = new List<string>();
            foreach (BlobContainerItem container in containers)
            {
                names.Add(container.Name);
            }
            return names;
        }

        public void OpenContainer(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task UploadBlobAsync(string filename, string path, bool overridefile = false)
        {
            var fullpath = Path.Combine(path, filename);
            var blobclient  = _blobContainerClient.GetBlobClient(filename);
            using FileStream uploadStream = File.OpenRead(fullpath);
            await blobclient.UploadAsync(uploadStream, overridefile);
            uploadStream.Close();
        }

        public async Task DownloadBlobAsync(string filename, string path)
        {
            var fullpath = Path.Combine(path, filename);
            var blobClient = _blobContainerClient.GetBlobClient(filename);
            await blobClient.DownloadToAsync(fullpath);
        }

        public async Task<List<string>> GetBlobNamesAsync()
        {
            List<string> names = new List<string>();

            await foreach(BlobItem blobItem in _blobContainerClient.GetBlobsAsync()) 
            {
                    names.Add(blobItem.Name);
            } 
            return names;
        }

        public async Task DeleteContainer()
        {
            await _blobContainerClient.DeleteAsync();
        }

        public async Task DeleteBlob(string name)
        {
            await _blobContainerClient.DeleteBlobIfExistsAsync(name);
        }
    }
}