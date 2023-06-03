using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlob
{
    internal class Program
    {
        static string folderPath = "C:\\Users\\Asad.Rizwan\\Desktop\\file\\input.mkv";
        static int chunkSize = 1024 * 1024; // 1 MB chunk size

        static async Task Main(String[] args)
        {
            string conn = "DefaultEndpointsProtocol=https;AccountName=asadblobstorage2014;AccountKey=k5wy5xwRgKzEQ++EWLv2abO/KNKvA9VuzIklcTiFnukRLnwljsF6eSovilEceEOGGDTgT0uOyk0V+ASt0ontCQ==;EndpointSuffix=core.windows.net";
            string containerName = "assignmentcontainer";
            string blobName = "output.mkv";

            BlobServiceClient blobServiceClient = new BlobServiceClient(conn);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            using (FileStream fs = new FileStream(folderPath, FileMode.Open, FileAccess.Read))
            {
                long fileSize = fs.Length;

                // Calculate the number of chunks needed
                int numChunks = (int)Math.Ceiling((double)fileSize / chunkSize);

                using (Stream blobStream = await blobClient.OpenWriteAsync(overwrite: true))
                {
                    for (int chunkIndex = 0; chunkIndex < numChunks; chunkIndex++)
                    {
                        byte[] buffer = new byte[chunkSize];
                        int bytesRead = fs.Read(buffer, 0, chunkSize);

                        await blobStream.WriteAsync(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine("File upload completed.");
            }
        }
    }
}