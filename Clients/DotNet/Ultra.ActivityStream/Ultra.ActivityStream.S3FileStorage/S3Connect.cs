using Amazon.S3;

namespace Ultra.ActivityStream.S3FileStorage
{
    public class S3Connect
    {
        public static IAmazonS3 Connect(string bucketUrl, string accessKey, string secretKey)
        {
            var s3Client = new AmazonS3Client(accessKey, secretKey, new AmazonS3Config
            {
                ServiceURL = bucketUrl,
                ForcePathStyle = true
            });

            return s3Client;
        }
    }
}