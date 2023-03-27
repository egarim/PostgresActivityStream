using Amazon.S3.Transfer;
using Amazon.S3;

namespace Ultra.ActivityStream.S3FileStorage
{
    public class S3Uploader
    {
        private readonly IAmazonS3 _s3Client;

        public S3Uploader(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
      
        //public async Task UploadFileAsync(string bucketName, string filePath, string keyName)
        //{
        //    using (var fileTransferUtility = new TransferUtility(_s3Client))
        //    {
        //        await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);

        //    }
        //}

        public async Task UploadFileAsync(string bucketName, Stream stream, string keyName)
        {
            using (var fileTransferUtility = new TransferUtility(_s3Client))
            {
                await fileTransferUtility.UploadAsync(stream, bucketName, keyName);
            }
        }

        //public async Task UploadFilesAsync(string bucketName, string[] filePaths)
        //{
        //    using (var fileTransferUtility = new TransferUtility(_s3Client))
        //    {
        //        await fileTransferUtility.UploadDirectoryAsync(@"C:\sample\folder", bucketName);
        //    }
        //}
    }
}