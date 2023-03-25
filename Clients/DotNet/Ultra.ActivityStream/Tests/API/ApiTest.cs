using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Infrastructure;
using Ultra.ActivityStream.Client;

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

        [Test]
        public async Task Test1()
        {
            var AppClient = HttpClientFactory.CreateClient("AppClient");
            var Response= await AppClient.GetAsync("simple");
            ActivityStreamClient activityStreamClient = new ActivityStreamClient(AppClient);
            activityStreamClient.UploadFilesAsync(new Ultra.ActivityStream.Contracts.StreamObject(),
                new Ultra.ActivityStream.Contracts.StreamObject(), new Ultra.ActivityStream.Contracts.StreamObject(), 0, 0, new List<Stream>());
        }
    }
}
