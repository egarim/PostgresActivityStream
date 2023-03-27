using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultra.ActivityStream.Contracts.Services;

namespace Ultra.ActivityStream.S3FileStorage
{
    public class S3FileStorageService : IFileStorageService
    {
        S3Uploader uploader;
        string BucketOrProxyUrl = "";
        string BucketName = "";
        public S3FileStorageService(S3Uploader uploader, string bucketOrProxyUrl, string bucketName)
        {
            this.uploader = uploader;
            BucketOrProxyUrl = bucketOrProxyUrl;
            BucketName = bucketName;
        }
        public S3FileStorageService(IAmazonS3 s3Client, string bucketOrProxyUrl, string bucketName)
        {
            uploader = new S3Uploader(s3Client);
            BucketOrProxyUrl = bucketOrProxyUrl;
            BucketName = bucketName;
        }
        public S3FileStorageService(string bucketUrl,string accessKey, string secretKey, string bucketOrProxyUrl, string bucketName)
        {
            uploader = new S3Uploader(S3Connect.Connect(bucketUrl, accessKey, secretKey));
            BucketOrProxyUrl = bucketOrProxyUrl;
            BucketName = bucketName;
        }
        public async Task<string> UploadAsync(Stream Data, string FileName)
        {
            await uploader.UploadFileAsync(this.BucketName,Data, FileName);
            return $"{BucketOrProxyUrl}/{FileName}";
        }
    }
}
