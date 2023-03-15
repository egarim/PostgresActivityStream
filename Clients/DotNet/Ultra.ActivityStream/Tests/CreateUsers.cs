using Newtonsoft.Json;
using Ultra.ActivityStream;

namespace Tests
{
    public class CreateUsers : TestBase
    {
        public override string SetConnectionStringName()
        {
            return nameof(CreateUsers);
        }

        [SetUp]
        public override Task Setup()
        {
            return base.Setup();
        }

        [Test]
        public async Task CreateUsersTest()
        {
            var SpbAccounts= AccountCreator.CreateAccountsNearStPetersburg();
            foreach (Account account in SpbAccounts)
            {
                await this.activityStreamClient.ObjectStorageUpsert(account);
            }

           await this.activityStreamClient.FollowObject(SpbAccounts.FirstOrDefault().Id, SpbAccounts.LastOrDefault().Id);
            // // command.CommandText = "SELECT * FROM as_get_objects_by_criteria_as_json('user', '2021-01-01', NULL, NULL, 1, 10, '59.942800,30.307100,10000');";

            var Objects=  await this.activityStreamClient.GetObjectsByCriteriaAsJson("user",null,null,null,1,10,null,null,null);

            var AccountsFromJson= JsonConvert.DeserializeObject<List<Account>>(Objects);

            var Objects2 = await this.activityStreamClient.GetObjectsByCriteriaAsObjects<List<Account>>("user", null, null, null, 1, 10, null, null, null);

            Assert.Pass();
        }
    }
}