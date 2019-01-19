using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;

namespace AzureImageUploadDemo
{
    public class ImageService
    {
        public async Task<string> ImageUpload(HttpPostedFileBase image)
        {
            string imagepath = null;
            try
            {
                StorageCredentials storageCredentials = new StorageCredentials("azutetestblob2065", "Ew+8lF5ACjxNZzr6j292eMloOZiARQgYNMdrNvghO3Eey8uRlsIwTVAvcp05zXJEI7oroyJREIzjJm1zi+gOlw==");
                CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("blobstoragedemo");
                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                        );
                }
                string imageName = Guid.NewGuid().ToString() + "-" + Path.GetExtension(image.FileName);

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = image.ContentType;
                await cloudBlockBlob.UploadFromStreamAsync(image.InputStream);

                imagepath = cloudBlockBlob.Uri.ToString();
            
            }
            catch(Exception ex)
            {

            }
            return imagepath;
        }
    }
}