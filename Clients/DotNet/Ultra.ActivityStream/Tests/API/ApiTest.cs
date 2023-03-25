using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Infrastructure;

namespace Tests.API
{
    public class ApiTest : MultiServerBaseTest
    {
        [SetUp()]
        public override void Setup()
        {
            base.Setup();
        }
        TestClientFactory HttpClientFactory;
        void Test1()
        {
            var AppClient = HttpClientFactory.CreateClient("AppClient");
        }
    }
}
