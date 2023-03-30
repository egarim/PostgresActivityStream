using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Infrastructure;
using Ultra.ActivityStream.Client;
using Ultra.ActivityStream.Contracts;
namespace Tests.API
{
    public class ApiTest : MultiServerBaseTest
    {
        [SetUp()]
        public override void Setup()
        {
            base.Setup();
            HttpClientFactory=this.GetTestClientFactory();
        }
        TestClientFactory HttpClientFactory;
        public static Stream GenerateRandomStream(int length)
        {
            var stream = new MemoryStream();

            var buffer = new byte[length];
            new Random().NextBytes(buffer);

            stream.Write(buffer, 0, buffer.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
        [Test]
        public async Task Test1()
        {
            var AppClient = HttpClientFactory.CreateClient("AppClient");
           


            ActivityStreamClient activityStreamClient = new ActivityStreamClient(AppClient);
            List<Stream> files = new List<Stream>();
            files.Add(GenerateRandomStream(100));
            files.Add(GenerateRandomStream(200));
            files.Add(GenerateRandomStream(300));

            //await activityStreamClient.CreateActivity(new StreamObject(),
            //    new StreamObject(), new StreamObject(), 0, 0, files);

            Dictionary<FileActivityStream, Stream> NewFiles = new Dictionary<FileActivityStream, Stream>();

            foreach (Stream stream in files)
            {
                NewFiles.Add(new FileActivityStream() { Id=Guid.NewGuid() }, stream);
            }
            await activityStreamClient.CreateObject(new StreamObject() { DisplayName="Test1" }, NewFiles);

            await activityStreamClient.FollowObjectFromIdsAsync(Guid.NewGuid(),Guid.NewGuid());
            await activityStreamClient.UnfollowObjectFromIdsAsync(Guid.NewGuid(), Guid.NewGuid());
        }
    }
}
